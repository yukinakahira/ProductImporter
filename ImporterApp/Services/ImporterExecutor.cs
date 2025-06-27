using ImporterApp.Models;
using ImporterApp.Infrastructure;
using ImporterApp.Services;

namespace ImporterApp
{
    // メインビジネスロジック
    public class ImporterExecutor
    {
        public void Execute(string csvPath, string userScenarioId)
        {
            try
            {
                // ステージングCSV読み込み
                var stagingData = CsvLoader.LoadCsv(csvPath);

                Logger.Info("[INFO] CSV Loaded :");
                foreach (var row in stagingData)
                {
                    var formattedRow = string.Join(", ", row.Select(kv => $"[{kv.Key}, {kv.Value}]"));
                    Logger.Info($" - {formattedRow}");
                }

                // ルールエンジンの初期化
                var rulesPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "rules.csv");
                var meaningRulesPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "meaning_rules.csv");
                var ruleEngine = new RuleEngine(rulesPath, meaningRulesPath);
                var importService = new ImportService(ruleEngine);

                foreach (var row in stagingData)
                {
                    try
                    {
                        var product = importService.ProcessRow(row, userScenarioId);

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
                        // 只输出一次summary
                        Logger.Info("=== Product Summary ===");
                        Logger.Info($"ProductCode : {product.ProductCode}");
                        Logger.Info($"Category : {product.Category}");
                        Logger.Info($"BrandId     : {product.BrandId}");
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
            }
            catch (Exception ex)
            {
                Logger.Info($"[FATAL] ImporterExecutor.Execute 例外: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
