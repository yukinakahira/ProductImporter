namespace ImporterApp.Models
{
    // 意味付け変換ルールモデル
    public class AttributeMeaningRule
    {
        public string AttributeId { get; set; } = string.Empty;        // 汎用属性名（例: SIZE_1）
        public string Usage { get; set; } = string.Empty;              // カテゴリ or 用途（例: バッグ、リング）
        public string MappedAttributeId { get; set; } = string.Empty;  // 意味を持たせた属性名（例: SIZE_VERTICAL, RING_SIZE）
    }
}