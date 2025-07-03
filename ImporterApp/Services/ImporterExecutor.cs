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
                var ProductList = new List<Product>();

                foreach (var row in stagingData)
                {
                    try
                    {
                        var product = importService.ProcessRow(row, usageId);
                        ProductList.Add(product);

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
                //修改map成功的代码，显示map类型，输出形式与失败时候类似Type={pending.PendingType}
                // マッピング成功のブランドリストを出力
                Logger.Info("=== マッピング成功のリスト（全件） ===");
                foreach (var product in ProductList)
                {
                    // 通过 PendingBrands 反查每个 Product 的哪些类型未失败，剩下的即为成功
                    var failedTypes = InMemoryProductRepository.PendingBrands
                        .Where(p => p.UsageId == product.ProductCode)
                        .Select(p => p.PendingType.ToUpper())
                        .ToHashSet();

                    // ブランド
                    if (!failedTypes.Contains("BRAND"))
                        Logger.Info($"[Mapped] ProductCode={product.ProductCode}, Type=BRAND, BrandId={product.BrandId}, BrandName={product.BrandName}");
                    // カテゴリ
                    if (!failedTypes.Contains("CATEGORY"))
                        Logger.Info($"[Mapped] ProductCode={product.ProductCode}, Type=CATEGORY, CategoryId={product.CategoryId}, CategoryName={product.CategoryName}");
                    // 項目リスト（属性）
                    if (product.Attributes != null && product.Attributes.Count > 0)
                    {
                        foreach (var attr in product.Attributes)
                        {
                            if (!failedTypes.Contains(attr.AttributeId.ToUpper()))
                                Logger.Info($"[Mapped] ProductCode={product.ProductCode}, Type={attr.AttributeId.ToUpper()}, AttributeId={attr.AttributeId}, Value={attr.Value}");
                        }
                    }
                }
                Logger.Info("==============================\n");

                // 全ての処理が完了した後、承認待ちブランドリストを出力
                if (InMemoryProductRepository.PendingBrands.Count > 0)
                {
                    Logger.Info("=== 承認待ちリスト（全件） ===");
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
