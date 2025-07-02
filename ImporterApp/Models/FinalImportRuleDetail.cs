namespace ImporterApp.Models
{
    // 生成された最終ルールを保存するホルダー
    // 区分的.xlsxファイルの内容を元に
    public class FinalImportRuleDetail
    {
        public string Usage { get; set; } = string.Empty;
        public string RuleId { get; set; } = string.Empty;
        public int ConditionSeq { get; set; }
        public int ColumnIndex { get; set; } // 1-based
        public string Operator { get; set; } = string.Empty;
        public string CompareValue { get; set; } = string.Empty;
        public string OutType { get; set; } = string.Empty;
        public string Logic { get; set; } = string.Empty;// AND/OR/None
        public string ResultValue { get; set; } = string.Empty;
        public string TargetTable { get; set; } = string.Empty;
        public string TargetColumn { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string Priority { get; set; }= string.Empty;
        public string Memo { get; set; } = string.Empty;
    }
}
