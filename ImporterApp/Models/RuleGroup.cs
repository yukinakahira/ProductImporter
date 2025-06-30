namespace ImporterApp.Models
{
    // 
    public class RuleCondition
    {
        public int ConditionSeq { get; set; }
        public int ColumnIndex { get; set; }
        public string? Operator { get; set; }
        public string? CompareValue { get; set; }
        public string? Logic { get; set; }
    }

    // 规则分组
    public class RuleGroup
    {
        public string? RuleId { get; set; }
        public string? OutType { get; set; }
        public string? ResultValue { get; set; }
        public string? TargetTable { get; set; }
        public string? TargetColumn { get; set; }
        public string? ItemId { get; set; }
        public List<RuleCondition> Conditions { get; set; } = new List<RuleCondition>();
    }
}
