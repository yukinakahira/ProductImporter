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
            // ステージングCSV読み込み
            var stagingData = CsvLoader.LoadCsv(csvPath);

            Logger.Info("[INFO] CSV Loaded :");
            foreach (var row in stagingData)
            {
                var formattedRow = string.Join(", ", row.Select(kv => $"[{kv.Key}, {kv.Value}]"));
                Logger.Info($" - {formattedRow}");
            }

            //行の順でステージングデータを処理
            var importService = new ImportService();

            foreach (var row in stagingData)
            {
                var product = importService.ProcessRow(row, userScenarioId);

                var errors = ProductValidator.Validate(product);
                if (errors.Any())
                {
                    foreach (var err in errors)
                        Logger.Info($"Validation: {err}");
                    continue;
                }

                // 差分チェックと履歴保存処理をここに追加
                var existing = InMemoryProductRepository.Products.FirstOrDefault(p => p.ProductCode == product.ProductCode);
                if (existing != null)
                {
                    var diffs = ProductDiffer.GetChangedFields(existing, product);
                    if (diffs.Any())
                    {
                        InMemoryProductRepository.Histories.Add(new ProductHistory
                        {
                            ProductCode = product.ProductCode,
                            ChangedAt = DateTime.Now,
                            ChangedFields = diffs
                        });
                        Logger.Info($"変更あり → 履歴保存: {string.Join(", ", diffs)}");
                    }
                    else
                    {
                        Logger.Info("差分なし → 履歴保存スキップ");
                    }
                }
                else
                {
                    InMemoryProductRepository.Products.Add(product);
                    Logger.Info("新規商品 → 初回登録（履歴保存なし）");
                }

                Logger.Info("=== Product Summary ===");
                Logger.Info($"ProductCode : {product.ProductCode}");
                Logger.Info($"category : {product.category}");
                Logger.Info($"BrandId     : {product.BrandId}");
                Logger.Info($"ProductName : {product.ProductName}");

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
        }
    }
}
