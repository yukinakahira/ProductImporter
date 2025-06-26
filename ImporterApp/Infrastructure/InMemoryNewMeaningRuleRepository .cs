using ImporterApp.Models;

namespace ImporterApp.Infrastructure
{

    public class InMemoryNewMeaningRuleRepository
    {
        public List<NewAttributeMeaningRule> GetNewRules()
        {
            // 
            return new List<NewAttributeMeaningRule>
            {
                new() { RuleId = "RULE_RKE_01", ConditionSeq = 1, ColumnIndex = 1, Operator = "=", CompareValue = "入出庫", Logic = "AND", OutType = "Transform",ResultValue = "06", TargetTable = "PRODUCT_MST", TargetColumn = "STATUS", ItemId = "STATUS" },
                new() { RuleId = "RULE_RKE_01", ConditionSeq = 1, ColumnIndex = 1, Operator = "=", CompareValue = "入出庫", Logic = "-", OutType = "Transform",ResultValue = "06", TargetTable = "PRODUCT_MST", TargetColumn = "STATUS", ItemId = "STATUS" },
                new() { RuleId = "RULE_RKE_02", ConditionSeq = 1, ColumnIndex = 3, Operator = "=", CompareValue = "売上", Logic = "-", OutType = "Fixed",ResultValue = "-", TargetTable = "PRODUCT_MST", TargetColumn = "STATUS", ItemId = "STATUS" },
                new() { RuleId = "RULE_KM_04", ConditionSeq = 1, ColumnIndex = 6, Operator = "=", CompareValue = "バッグ", Logic = "-", OutType = "Fixed",ResultValue = "-", TargetTable = "PRODUCT_EAV", TargetColumn = "VALUE", ItemId = "BAG_WIDTH" }
            };
        }
    }
}