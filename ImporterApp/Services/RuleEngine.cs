using ImporterApp.Models;
using System.Text.RegularExpressions;

namespace ImporterApp.Services
{
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
                    // 対象列の値を取得（ColumnIndexは1始まりのため -1 する）
                    var fieldValue = record.ChangedFields[rule.ColumnIndex - 1];

                    // Transform の場合は比較演算が必要
                    if (rule.OutType == "Transform")
                    {
                        match &= Evaluate(fieldValue, rule.Operator, rule.CompareValue);

                        // AND 論理で一致しなければ途中で終了
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
                        Console.WriteLine($"[RULE:{first.RuleId}] 条件一致 → {first.ItemId} = {first.ResultValue}");
                    }

                    // Fixed → 比較なしでCSV値をそのままセット
                    if (first.OutType == "Fixed")
                    {
                        var fieldValue = record.ChangedFields[first.ColumnIndex - 1];
                        SaveResult(record, first.TargetTable, first.TargetColumn, first.ItemId, fieldValue);
                        Console.WriteLine($"[RULE:{first.RuleId}] 固定値登録 → {first.ItemId} = {fieldValue}");
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
    }
}
