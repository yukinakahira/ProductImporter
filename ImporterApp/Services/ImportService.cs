using ImporterApp.Models;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly InMemoryRuleRepository _ruleRepo;

        public ImportService()
        {
            _ruleRepo = new InMemoryRuleRepository();
        }

        public Product ProcessRow(Dictionary<string, string> rowData, string userScenarioId)
        {
            Logger.Info($"[INFO] Start processing row: {string.Join(", ", rowData)}");

            // ① ルール取得
            var ruleDetails = _ruleRepo.GetRules(userScenarioId);
            Logger.Info($"[INFO] Rules loaded: {ruleDetails.Count}");

            // ② Product初期化
            var product = new Product();

            // ③ 各ルールに従ってマッピング適用
            foreach (var rule in ruleDetails)
            {
                if (!rowData.TryGetValue(rule.ColumnName, out var value)) continue;

                Logger.Info($"[INFO] Applying ColumnName: {rule.ColumnName} → AttributeId: {rule.AttributeId} = {value}");

                if (rule.SaveTable == "PRODUCT_MST")
                {
                    switch (rule.AttributeId)
                    {
                        case "PRODUCT_CODE":
                            product.ProductCode = value;
                            break;
                        case "BRAND_ID":
                            product.BrandId = value;
                            break;
                        case "PRODUCT_NAME":
                            product.ProductName = value;
                            break;
                    }

                    Logger.Info($"[INFO] Set {rule.AttributeId} = {value}");
                }
                else if (rule.SaveTable == "PRODUCT_EAV")
                {
                    product.Attributes.Add(new ProductAttribute
                    {
                        AttributeId = rule.AttributeId,
                        Value = value
                    });

                    Logger.Info($"[INFO] Set EAV Attribute: {rule.AttributeId} = {value}");
                }
            }

            return product;
        }
    }
}
