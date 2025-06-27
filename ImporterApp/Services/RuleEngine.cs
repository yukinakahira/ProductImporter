using ImporterApp.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{
    /// <summary>
    /// ルールエンジンのインターフェース
    /// </summary>
    public interface IRuleEngine
    {
        List<RuleGroup> Rules { get; }
        List<AttributeMeaningRule> MeaningRules { get; }
        void MapMainProperties(Product product, Dictionary<string, string> rowData);
        string MapAttributeId(string itemId, string category);
        bool EvaluateRule(RuleGroup rule, Dictionary<string, string> rowData);
    }

    /// <summary>
    /// CSVベースのルールエンジン実装
    /// </summary>
    public class RuleEngine : IRuleEngine
    {
        public List<RuleGroup> Rules { get; }
        public List<AttributeMeaningRule> MeaningRules { get; }

        public RuleEngine(string rulesPath, string meaningRulesPath)
        {
            // ルールCSV読み込み
            var rawRules = CsvLoaderUtil.LoadFromCsv(rulesPath, cols =>
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

            // 输出所有原始规则（结构化、分行、带标签）
            Logger.Info("========== [RULES] Raw rules loaded from CSV ==========");
            foreach (var rule in rawRules)
            {
                Logger.Info($"--- Rule ---");
                Logger.Info($"  RuleId        : {rule.RuleId}");
                Logger.Info($"  ItemId        : {rule.ItemId}");
                Logger.Info($"  TargetTable   : {rule.TargetTable}");
                Logger.Info($"  TargetColumn  : {rule.TargetColumn}");
                Logger.Info($"  OutType       : {rule.OutType}");
                Logger.Info($"  ResultValue   : {rule.ResultValue}");
                Logger.Info($"  ConditionSeq  : {rule.ConditionSeq}");
                Logger.Info($"  ColumnIndex   : {rule.ColumnIndex}");
                Logger.Info($"  Operator      : {rule.Operator}");
                Logger.Info($"  CompareValue  : {rule.CompareValue}");
                Logger.Info($"  Logic         : {rule.Logic}");
            }
            Logger.Info("======================================================");

            Rules = GroupRules(rawRules);

            // 输出所有分组后的规则组（结构化、分行、带标签）
            Logger.Info("========== [RULE GROUPS] Grouped rules ==========");
            foreach (var group in Rules)
            {
                Logger.Info($"--- RuleGroup ---");
                Logger.Info($"  RuleId        : {group.RuleId}");
                Logger.Info($"  ItemId        : {group.ItemId}");
                Logger.Info($"  TargetTable   : {group.TargetTable}");
                Logger.Info($"  TargetColumn  : {group.TargetColumn}");
                Logger.Info($"  OutType       : {group.OutType}");
                Logger.Info($"  ResultValue   : {group.ResultValue}");
                Logger.Info($"  [Conditions]  :");
                foreach (var cond in group.Conditions)
                {
                    Logger.Info($"    - Seq        : {cond.ConditionSeq}");
                    Logger.Info($"      ColIdx     : {cond.ColumnIndex}");
                    Logger.Info($"      Operator   : {cond.Operator}");
                    Logger.Info($"      CompareVal : {cond.CompareValue}");
                    Logger.Info($"      Logic      : {cond.Logic}");
                }
            }
            Logger.Info("===============================================");

            // 意味マッピングルール読み込み
            MeaningRules = CsvLoaderUtil.LoadFromCsv(meaningRulesPath, cols =>
                cols.Length < 3 ? null : new AttributeMeaningRule
                {
                    AttributeId = cols[0],
                    Usage = cols[1],
                    MappedAttributeId = cols[2]
                }).Where(x => x != null).ToList();
        }

        // ルールをRuleGroup単位にまとめる
        private List<RuleGroup> GroupRules(List<NewAttributeMeaningRule> rules)
        {
            var grouped = new List<RuleGroup>();
            var currentGroup = new RuleGroup();

            foreach (var rule in rules.OrderBy(r => r.RuleId).ThenBy(r => r.ConditionSeq))
            {
                var isNewGroup = string.IsNullOrEmpty(currentGroup.RuleId) || currentGroup.RuleId != rule.RuleId;

                if (isNewGroup || currentGroup.Conditions.Count == 0)
                {
                    currentGroup = new RuleGroup
                    {
                        RuleId = rule.RuleId,
                        OutType = rule.OutType,
                        ResultValue = rule.ResultValue,
                        TargetTable = rule.TargetTable,
                        TargetColumn = rule.TargetColumn,
                        ItemId = rule.ItemId,
                        Conditions = new List<RuleCondition>()
                    };
                }

                currentGroup.Conditions.Add(new RuleCondition
                {
                    ConditionSeq = rule.ConditionSeq,
                    ColumnIndex = rule.ColumnIndex,
                    Operator = rule.Operator,
                    CompareValue = rule.CompareValue,
                    Logic = rule.Logic
                });

                if (string.IsNullOrEmpty(rule.Logic) || rule.Logic == "-")
                {
                    grouped.Add(currentGroup);
                    currentGroup = new RuleGroup();
                }
            }
            return grouped;
        }


        public void MapMainProperties(Product product, Dictionary<string, string> rowData)
        {
            var mainKeys = new[]
            {
                new { Key = "PRODUCT_CODE", Setter = new Action<string>(v => product.ProductCode = v) },
                new { Key = "BRAND_ID", Setter = new Action<string>(v => product.BrandId = v) },
                new { Key = "PRODUCT_NAME", Setter = new Action<string>(v => product.ProductName = v) }
            };

            foreach (var main in mainKeys)
            {
                var rule = Rules.FirstOrDefault(r => r.ItemId == main.Key && r.TargetTable == "PRODUCT_MST");
                if (rule != null)
                {
                    bool hit = EvaluateRule(rule, rowData);
                    // 用 MapAttributeId 做业务名到物理列名的映射
                    var mappedCol = MapAttributeId(main.Key, "");
                    string val = null;
                    if (!string.IsNullOrEmpty(rule.TargetColumn) && rowData.TryGetValue(rule.TargetColumn, out var v1))
                        val = v1;
                    else if (!string.IsNullOrEmpty(mappedCol) && rowData.TryGetValue(mappedCol, out var v2))
                        val = v2;
                    else if (rowData.TryGetValue(main.Key, out var v3))
                        val = v3;
                    Logger.Info($"[DEBUG] MapMainProperties: key={main.Key}, mappedCol={mappedCol}, rule.TargetColumn={rule.TargetColumn}, EvaluateHit={hit}, value='{val}'");
                    if (hit)
                        main.Setter(val ?? string.Empty);
                }
                else
                {
                    Logger.Info($"[DEBUG] MapMainProperties: key={main.Key}, rule not found");
                }
            }
            Logger.Info($"[DEBUG] Mapped properties for product: {product.ProductCode}, {product.BrandId}, {product.ProductName}");
        }

        public string MapAttributeId(string itemId, string category)
        {
            Logger.Info($"[DEBUG] Mapping attribute: {itemId} with category: {category}");
            return MeaningRules.FirstOrDefault(r => r.AttributeId == itemId && r.Usage == category)?.MappedAttributeId ?? itemId;
        }

        public bool EvaluateRule(RuleGroup rule, Dictionary<string, string> rowData)
        {
            bool match = true;

            foreach (var cond in rule.Conditions)
            {
                string fieldValue = GetFieldValue(cond.ColumnIndex, rowData, rule.TargetColumn);
                if (!string.IsNullOrEmpty(cond.Operator))
                {
                    match &= Evaluate(fieldValue, cond.Operator, cond.CompareValue);
                    if (!match && cond.Logic == "AND") break;
                }
            }
            return match;
        }

        private string GetFieldValue(int columnIndex, Dictionary<string, string> rowData, string fallbackKey)
        {
            if (columnIndex > 0)
            {
                var key = rowData.Keys.ElementAtOrDefault(columnIndex - 1);
                return key != null && rowData.TryGetValue(key, out var val) ? val : string.Empty;
            }
            return rowData.TryGetValue(fallbackKey, out var fallbackVal) ? fallbackVal : string.Empty;
        }

        private bool Evaluate(string value, string op, string compare)
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
    }
}
