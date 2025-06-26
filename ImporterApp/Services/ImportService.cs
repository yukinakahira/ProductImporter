using ImporterApp.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly List<NewAttributeMeaningRule> _rules;
        private readonly List<AttributeMeaningRule> _meaningRules;
        private readonly RuleEngine _ruleEngine;

        public ImportService()
        {
            // 规则和meaning规则用CsvLoaderUtil加载
            var csvPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "rules.csv");
            _rules = CsvLoaderUtil.LoadFromCsv(csvPath, cols =>
                cols.Length < 14 ? null : new NewAttributeMeaningRule {
                    RuleId = cols[3],
                    ConditionSeq = int.TryParse(cols[4], out var seq) ? seq : 0,
                    ColumnIndex = int.TryParse(cols[5], out var idx) ? idx : 0,
                    Operator = cols[6],
                    CompareValue = cols[7],
                    Logic = cols[8],
                    OutType = cols[9],
                    ResultValue = cols[10],
                    TargetTable = cols[11],
                    TargetColumn = cols[12],
                    ItemId = cols[13]
                }).Where(x => x != null).ToList();
            var meaningCsvPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "meaning_rules.csv");
            _meaningRules = CsvLoaderUtil.LoadFromCsv(meaningCsvPath, cols =>
                cols.Length < 3 ? null : new AttributeMeaningRule {
                    AttributeId = cols[0],
                    Usage = cols[1],
                    MappedAttributeId = cols[2]
                }).Where(x => x != null).ToList();
            _ruleEngine = new RuleEngine(_rules);
        }

        // staging.csv等数据行读取建议在外部用CsvLoader.LoadCsv
        // 这里只处理业务映射
        public Product ProcessRow(Dictionary<string, string> rowData, string userScenarioId)
        {
            var ruleDetails = _rules; // 可根据userScenarioId过滤
            var product = new Product();

            // 主属性赋值
            var productCodeRule = ruleDetails.FirstOrDefault(r => r.ItemId == "PRODUCT_CODE" && r.TargetTable == "PRODUCT_MST");
            if (productCodeRule != null && rowData.TryGetValue(productCodeRule.TargetColumn, out var productCodeValue))
                product.ProductCode = productCodeValue;
            var brandIdRule = ruleDetails.FirstOrDefault(r => r.ItemId == "BRAND_ID" && r.TargetTable == "PRODUCT_MST");
            if (brandIdRule != null && rowData.TryGetValue(brandIdRule.TargetColumn, out var brandIdValue))
                product.BrandId = brandIdValue;
            var productNameRule = ruleDetails.FirstOrDefault(r => r.ItemId == "PRODUCT_NAME" && r.TargetTable == "PRODUCT_MST");
            if (productNameRule != null && rowData.TryGetValue(productNameRule.TargetColumn, out var productNameValue))
                product.ProductName = productNameValue;

            // 优先直接从COL_7赋值category
            if (rowData.TryGetValue("COL_7", out var directCategoryValue))
                product.category = directCategoryValue;
            else
            {
                var categoryRule = ruleDetails.FirstOrDefault(r => r.ItemId == "CATEGORY" && r.TargetTable == "PRODUCT_MST");
                if (categoryRule != null && rowData.TryGetValue(categoryRule.TargetColumn, out var categoryValue))
                    product.category = categoryValue;
            }

            // 属性映射
            foreach (var rule in ruleDetails)
            {
                if (!rowData.TryGetValue(rule.TargetColumn, out var value)) continue;
                if (rule.TargetTable == "PRODUCT_MST") continue;
                if (rule.TargetTable == "PRODUCT_EAV")
                {
                    var mappedId = RuleEngine.MapAttributeId(rule.ItemId, product.category, _meaningRules);
                    product.Attributes.Add(new ProductAttribute
                    {
                        AttributeId = mappedId,
                        Value = value
                    });
                }
            }
            return product;
        }
    }
}
