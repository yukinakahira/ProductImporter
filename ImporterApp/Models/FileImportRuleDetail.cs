namespace ImporterApp.Models
{
    // ファイル取込ルール詳細
    public class FileImportRuleDetail
    {
        public string UseCaseId { get; set; } = string.Empty;
        public string ColumnName { get; set; } = string.Empty;
        public string AttributeId { get; set; } = string.Empty;
        public string ValuePrefix { get; set; } = string.Empty;
        public string SaveTable { get; set; } = string.Empty;
        public string SaveColumn { get; set; } = string.Empty;
        public bool EavFlag { get; set; }
    }
}
