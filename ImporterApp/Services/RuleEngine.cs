using ImporterApp.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{

    public interface IRuleEngine
    {
        List<NewAttributeMeaningRule> Rules { get; }
        List<AttributeMeaningRule> MeaningRules { get; }
        void MapMainProperties(Product product, Dictionary<string, string> rowData);
        string MapAttributeId(string itemId, string category);
    }

    public class CsvRuleEngine : IRuleEngine
    {
        public List<NewAttributeMeaningRule> Rules { get; }
        public List<AttributeMeaningRule> MeaningRules { get; }

        // コンストラクタでCSVファイルからルールを読み込む
        // rulesPath: 新属性の意味ルールCSVファイルのパス
        public CsvRuleEngine(string rulesPath, string meaningRulesPath)
        {
            Rules = CsvLoaderUtil.LoadFromCsv(rulesPath, cols =>
                cols.Length < 14 ? null : new NewAttributeMeaningRule
                {
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
            MeaningRules = CsvLoaderUtil.LoadFromCsv(meaningRulesPath, cols =>
                cols.Length < 3 ? null : new AttributeMeaningRule
                {
                    AttributeId = cols[0],
                    Usage = cols[1],
                    MappedAttributeId = cols[2]
                }).Where(x => x != null).ToList();
        }

        // 主属性（ProductCode、BrandId、ProductName）をマッピング
        // rowData: ステージングデータの行データ（Dictionary形式）
        public void MapMainProperties(Product product, Dictionary<string, string> rowData)
        {
            var productCodeRule = Rules.FirstOrDefault(r => r.ItemId == "PRODUCT_CODE" && r.TargetTable == "PRODUCT_MST");
            if (productCodeRule != null && rowData.TryGetValue(productCodeRule.TargetColumn, out var productCodeValue))
                product.ProductCode = productCodeValue;
            var brandIdRule = Rules.FirstOrDefault(r => r.ItemId == "BRAND_ID" && r.TargetTable == "PRODUCT_MST");
            if (brandIdRule != null && rowData.TryGetValue(brandIdRule.TargetColumn, out var brandIdValue))
                product.BrandId = brandIdValue;
            var productNameRule = Rules.FirstOrDefault(r => r.ItemId == "PRODUCT_NAME" && r.TargetTable == "PRODUCT_MST");
            if (productNameRule != null && rowData.TryGetValue(productNameRule.TargetColumn, out var productNameValue))
                product.ProductName = productNameValue;
        }

        public string MapAttributeId(string itemId, string category)
        {
            return MeaningRules.FirstOrDefault(r => r.AttributeId == itemId && r.Usage == category)?.MappedAttributeId ?? itemId;
        }
    }
    
    public class RuleEngine
    {
        private readonly List<NewAttributeMeaningRule> _rules;

        public RuleEngine(List<NewAttributeMeaningRule> rules)
        {
            _rules = rules;
        }

        // ルール適用処理（行データに対してルールを評価し、結果を記録）
        public void ApplyRules(ProductHistory record)
        {
            foreach (var ruleGroup in _rules.GroupBy(r => r.RuleId))
            {
                bool match = true;

                foreach (var rule in ruleGroup)
                {
                    string fieldValue = null;
                    // 优先用ChangedFieldValues查找字段值
                    if (!string.IsNullOrEmpty(rule.TargetColumn) && record.ChangedFieldValues != null && record.ChangedFieldValues.TryGetValue(rule.TargetColumn, out var val))
                    {
                        fieldValue = val;
                    }
                    else if (rule.ColumnIndex > 0 && rule.ColumnIndex <= record.ChangedFields.Count)
                    {
                        // 兼容旧逻辑：如果 ChangedFields 是有序的原始字段值
                        fieldValue = record.ChangedFields[rule.ColumnIndex - 1];
                    }
                    else if (!string.IsNullOrEmpty(rule.TargetColumn) && record.ChangedFields.Contains(rule.TargetColumn))
                    {
                        // 如果 ChangedFields 存字段名，则用 TargetColumn 查找
                        fieldValue = rule.TargetColumn;
                    }
                    else
                    {
                        Logger.Info($"[WARN] ルール[{rule.RuleId}]の対象列({rule.TargetColumn})がChangedFields/ChangedFieldValuesに存在しません。スキップします。");
                        continue;
                    }

                    // Transform の場合は比較演算が必要
                    if (rule.OutType == "Transform")
                    {
                        match &= Evaluate(fieldValue, rule.Operator, rule.CompareValue);
                        if (!match && rule.Logic == "AND") break;
                    }
                }

                if (match)
                {
                    var first = ruleGroup.First();

                    // Transform → 条件一致なら ResultValue をセット
                    if (first.OutType == "Transform")
                    {
                        SaveResult(record, first.TargetTable, first.TargetColumn, first.ItemId, first.ResultValue);
                        Logger.Info($"[RULE:{first.RuleId}] TRANSFORM: {first.ItemId} ← {first.ResultValue} (from {first.TargetColumn})");
                        // 这里可模拟后续开发需要的操作，如调用API、写DB等
                    }

                    // Fixed → 比較なしでCSV値をそのままセット
                    if (first.OutType == "Fixed")
                    {
                        string fieldValue = null;
                        if (!string.IsNullOrEmpty(first.TargetColumn) && record.ChangedFieldValues != null && record.ChangedFieldValues.TryGetValue(first.TargetColumn, out var val))
                        {
                            fieldValue = val;
                        }
                        else if (first.ColumnIndex > 0 && first.ColumnIndex <= record.ChangedFields.Count)
                        {
                            fieldValue = record.ChangedFields[first.ColumnIndex - 1];
                        }
                        else if (!string.IsNullOrEmpty(first.TargetColumn) && record.ChangedFields.Contains(first.TargetColumn))
                        {
                            fieldValue = first.TargetColumn;
                        }
                        Logger.Info($"[RULE:{first.RuleId}] FIXED: {first.ItemId} ← {fieldValue} (from {first.TargetColumn})");
                        SaveResult(record, first.TargetTable, first.TargetColumn, first.ItemId, fieldValue);
                        // 这里可模拟后续开发需要的操作，如调用API、写DB等
                    }
                }
            }
        }

        // 演算子に基づいて比較を行う
        public bool Evaluate(string value, string op, string compare)
        {
            return op switch
            {
                "=" => value == compare,
                "<>" => value != compare,
                ">" => double.TryParse(value, out var d1) && double.TryParse(compare, out var d2) && d1 > d2,
                "<" => double.TryParse(value, out var d3) && double.TryParse(compare, out var d4) && d3 < d4,
                "LIKE" => value.Contains(compare),
                "TRUE" => value.ToUpper() == "TRUE",
                _ => false
            };
        }

        // 結果値を保存（PRODUCT_MST or PRODUCT_EAV に応じて記録形式を変える）
        private void SaveResult(ProductHistory record, string table, string column, string itemId, string value)
        {
            if (table == "PRODUCT_MST")
            {
                // 商品マスタの属性値（例：STATUS列）を記録（ここでは汎用的な辞書形式に）
                record.Result[$"{table}.{column}"] = value;
            }
            else if (table == "PRODUCT_EAV")
            {
                // EAV形式で itemId に対して値を記録
                record.Result[itemId] = value;
            }
        }

        // 属性IDの意味マッピング（AttributeMeaningMapper.Mapの代替）
        public static string MapAttributeId(string attributeId, string usage, List<AttributeMeaningRule> rules)
        {
            return rules
                .FirstOrDefault(r => r.AttributeId == attributeId && r.Usage == usage)
                ?.MappedAttributeId ?? attributeId;
        }

        // 主属性（ProductCode、BrandId、ProductName）提取
        public static void MapMainProperties(Product product, Dictionary<string, string> rowData, List<NewAttributeMeaningRule> rules)
        {
            var productCodeRule = rules.FirstOrDefault(r => r.ItemId == "PRODUCT_CODE" && r.TargetTable == "PRODUCT_MST");
            if (productCodeRule != null && rowData.TryGetValue(productCodeRule.TargetColumn, out var productCodeValue))
                product.ProductCode = productCodeValue;
            var brandIdRule = rules.FirstOrDefault(r => r.ItemId == "BRAND_ID" && r.TargetTable == "PRODUCT_MST");
            if (brandIdRule != null && rowData.TryGetValue(brandIdRule.TargetColumn, out var brandIdValue))
                product.BrandId = brandIdValue;
            var productNameRule = rules.FirstOrDefault(r => r.ItemId == "PRODUCT_NAME" && r.TargetTable == "PRODUCT_MST");
            if (productNameRule != null && rowData.TryGetValue(productNameRule.TargetColumn, out var productNameValue))
                product.ProductName = productNameValue;
        }
    }
}
