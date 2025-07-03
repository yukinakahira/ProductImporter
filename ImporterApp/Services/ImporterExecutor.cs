using ImporterApp.Models;
using ImporterApp.Infrastructure;
using ImporterApp.Rules;
using ImporterApp.Services;

namespace ImporterApp
{
    // メインビジネスロジック
    public class ImporterExecutor
    {
        public void Execute(string csvPath, string groupCompanyId)
        {
            try
            {
                Logger.Info("[INFO] グループ会社ID: " + groupCompanyId + " を使用して処理を開始します。");
                // ステージングCSV読み込み
                var stagingData = CsvLoader.LoadCsv(csvPath);
                // ファイル取込ルールでのUsageId取得
                var usageId = InMemoryFileImportRule.GetUsageId(System.IO.Path.GetFileName(csvPath), groupCompanyId);
                if (string.IsNullOrEmpty(usageId))
                {
                    Logger.Error($"[ERROR] ファイル取込ルールが見つかりません: {csvPath}, グループ会社ID: {groupCompanyId}");
                    return;
                }

                Logger.Info("[INFO] CSV Loaded :");
                foreach (var row in stagingData)
                {
                    var formattedRow = string.Join(", ", row.Select(kv => $"[{kv.Key}, {kv.Value}]"));
                    Logger.Info($" - {formattedRow}");
                }

                // ルールエンジンの初期化
                var rulesPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "rules.csv");
                var ruleEngine = new RuleEngine(rulesPath);
                var importService = new ImportService(ruleEngine);

                foreach (var row in stagingData)
                {
                    try
                    {
                        var product = importService.ProcessRow(row, usageId);

                        var errors = ProductValidator.Validate(product);
                        if (errors.Any())
                        {
                            foreach (var err in errors)
                                Logger.Info($"Validation: {err}");
                            continue;
                        }

                        // 差分チェックと履歴保存処理
                        var existing = InMemoryProductRepository.Products.FirstOrDefault(p => p.ProductCode == product.ProductCode);
                        if (existing != null)
                        {
                            var diffs = ProductDiffer.GetChangedFields(existing, product);
                            var diffValues = ProductDiffer.GetChangedFieldValues(existing, product);
                            if (diffs.Any())
                            {
                                var history = new ProductHistory
                                {
                                    ProductCode = product.ProductCode,
                                    ChangedAt = DateTime.Now,
                                    ChangedFields = diffs,
                                    ChangedFieldValues = diffValues
                                };
                                InMemoryProductRepository.Histories.Add(history);
                                Logger.Info($"変更あり → 履歴保存: {string.Join(", ", diffs)}");
                            }
                            else
                            {
                                Logger.Info("差分なし → 履歴保存スキップ");
                            }
                        }
                        else if (!InMemoryProductRepository.Products.Any(p => p.ProductCode == product.ProductCode))
                        {
                            InMemoryProductRepository.Products.Add(product);
                            Logger.Info("新規商品 → 初回登録（履歴保存なし）");
                        }
                        // ここで商品情報をログに出力
                        Logger.Info("=== Product Summary ===");
                        Logger.Info($"ProductCode : {product.ProductCode}");
                        Logger.Info($"CategoryId : {product.CategoryId}");
                        Logger.Info($"CategoryName : {product.CategoryName}");
                        Logger.Info($"BrandId     : {product.BrandId}");
                        Logger.Info($"BrandName : {product.BrandName}");
                        Logger.Info($"ProductName : {product.ProductName}");
                        Logger.Info($"State : {product.State}");

                        if (product.Attributes.Any())
                        {
                            Logger.Info("Attributes  :");
                            foreach (var attr in product.Attributes)
                            {
                                Logger.Info($"  - {attr.AttributeId} = {attr.Value}");
                            }
                        }
                        else
                        {
                            Logger.Info("Attributes  : (none)");
                        }
                        Logger.Info("=======================");
                    }
                    catch (Exception ex)
                    {
                        Logger.Info($"[FATAL] 行単位処理例外: {ex.Message}\n{ex.StackTrace}");
                        // 行単位のエラーはログに記録し、次の行へ進む
                    }
                }
                // マッピング成功のブランドリストを出力
                Logger.Info("=== マッピング成功のブランドリスト（全件） ===");
                foreach (var product in InMemoryProductRepository.Products)
                {
                    Logger.Info($"[Mapped] ProductCode={product.ProductCode}, BrandId={product.BrandId}, Category={product.CategoryName}, ProductName={product.ProductName}");
                }
                Logger.Info("==============================\n");

                // 全ての処理が完了した後、承認待ちブランドリストを出力
                if (InMemoryProductRepository.PendingBrands.Count > 0)
                {
                    Logger.Info("=== 承認待ちブランドリスト（全件） ===");
                    foreach (var pending in InMemoryProductRepository.PendingBrands)
                    {
                        Logger.Info($"[ApprovalPending] Type={pending.PendingType}, PendingId={pending.PendingId},OriginalId={pending.OriginalId}, OriginalName={pending.OriginalName},ProductCode={pending.UsageId}, Remarks={pending.Remarks}");
                    }
                    Logger.Info("==============================\n");
                }
            }
            catch (Exception ex)
            {
                Logger.Info($"[FATAL] ImporterExecutor.Execute 例外: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
