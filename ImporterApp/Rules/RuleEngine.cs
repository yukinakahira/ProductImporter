using ImporterApp.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using ImporterApp.Infrastructure;
using ImporterApp.Services;

namespace ImporterApp.Rules
{
    /// <summary>
    /// CSVベースのルールエンジン実装
    /// </summary>
    public class RuleEngine
    {
        public List<RuleGroup> Rules { get; }
        public List<AttributeMeaningRule> MeaningRules { get; }

        public RuleEngine(string rulesPath)
        {
            // ルールCSV読み込み
            var rawRules = CsvLoaderUtil.LoadFromCsv(rulesPath, cols =>
                cols.Length < 15 ? null : new FinalImportRuleDetail
                {
                    Usage = cols[1],
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
                    ItemId = cols[13],
                    Memo = cols[14]
                }).Where(x => x != null).ToList();

            // 输出所有原始规则（结构化、分行、带标签）
            Logger.Info("========== [RULES] Raw rules loaded from CSV ==========");
            foreach (var rule in rawRules)
            {
                Logger.Info($"--- Rule ---");
                Logger.Info($"  Usage         : {rule.Usage}");
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
                Logger.Info($"  Memo          : {rule.Memo}");
            }
            Logger.Info("======================================================");

            Rules = GroupRules(rawRules);

            // 输出所有分组后的规则组（结构化、分行、带标签）
            Logger.Info("========== [RULE GROUPS] Grouped rules ==========");
            foreach (var group in Rules)
            {
                Logger.Info($"--- RuleGroup ---");
                Logger.Info($"  RuleId        : {group.RuleId}");
                Logger.Info($"  Usage         : {group.Usage}");
                Logger.Info($"  ItemId        : {group.ItemId}");
                Logger.Info($"  TargetTable   : {group.TargetTable}");
                Logger.Info($"  TargetColumn  : {group.TargetColumn}");
                Logger.Info($"  OutType       : {group.OutType}");
                Logger.Info($"  ResultValue   : {group.ResultValue}");
                Logger.Info($"  Priority  : {group.Priority}");
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
        }

        // ルールをRuleGroup単位にまとめる
        private List<RuleGroup> GroupRules(List<FinalImportRuleDetail> rules)
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
                        Usage = rule.Usage,
                        RuleId = rule.RuleId,
                        OutType = rule.OutType,
                        ResultValue = rule.ResultValue,
                        TargetTable = rule.TargetTable,
                        TargetColumn = rule.TargetColumn,
                        ItemId = rule.ItemId,
                        Priority = rule.Priority ?? string.Empty,
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
         public string MapAttributeId(string itemId, string category)
         {
             Logger.Info($"[DEBUG] Mapping attribute: {itemId} with category: {category}");
             return MeaningRules.FirstOrDefault(r => r.AttributeId == itemId && r.Usage == category)?.MappedAttributeId ?? itemId;
         }
    }
}
