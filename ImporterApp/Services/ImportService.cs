using ImporterApp.Models;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly InMemoryRuleRepository _ruleRepo;
        private readonly InMemoryMeaningRuleRepository _meaningRuleRepo;

        public ImportService()
        {
            _ruleRepo = new InMemoryRuleRepository();
            _meaningRuleRepo = new InMemoryMeaningRuleRepository();
        }

        public Product ProcessRow(Dictionary<string, string> rowData, string userScenarioId)
        {
            Logger.Info($"[INFO] Start processing row: {string.Join(", ", rowData)}");

            // ① ルール取得
            var ruleDetails = _ruleRepo.GetRules(userScenarioId);
            Logger.Info($"[INFO] Rules loaded: {ruleDetails.Count}");

            // ② Product初期化
            var product = new Product();

            // ★ 先にカテゴリだけ取得しておく（意味マッピングの前提になる）
            var categoryRule = ruleDetails.FirstOrDefault(r => r.AttributeId == "CATEGORY" && r.SaveTable == "PRODUCT_MST");
            if (categoryRule != null && rowData.TryGetValue(categoryRule.ColumnName, out var categoryValue))
            {
                product.category = categoryValue;
                Logger.Info($"[INFO] Set CATEGORY = {categoryValue}");
            }

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
                        case "CATEGORY":
                            product.category = value;
                            break;
                    }

                    Logger.Info($"[INFO] Set {rule.AttributeId} = {value}");
                }
                else if (rule.SaveTable == "PRODUCT_EAV")
                {
                    // 通常の属性を一旦登録
                     var attr = new ProductAttribute
                     {
                         AttributeId = rule.AttributeId,
                         Value = value
                     };
                     product.Attributes.Add(attr);

                    // カテゴリに応じた意味変換ルール適用（例：SIZE_1 → SIZE_VERTICAL）

                    if (!string.IsNullOrWhiteSpace(product.category))
                    {
                        var mappedId = AttributeMeaningMapper.Map(rule.AttributeId, product.category, _meaningRuleRepo.GetMeaningRules());
                        if (mappedId != rule.AttributeId)
                        {
                            product.Attributes.Add(new ProductAttribute
                            {
                                AttributeId = mappedId,
                                Value = value
                            });
                            Logger.Info($"[INFO] Semantic Mapping Applied: {rule.AttributeId} → {mappedId} = {value}");
                        }
                    }
                   // Logger.Info($"[INFO] Set EAV Attribute: {attr.AttributeId} = {attr.Value}");
                }
            }

            return product;
        }
    }
}
