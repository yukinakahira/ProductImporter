namespace ImporterApp.Models
{
    // 履歴情報モデル
    public class ProductHistory
    {
        public string ProductCode { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public List<string> ChangedFields { get; set; } = new();
        public Dictionary<string, string> ChangedFieldValues { get; set; } = new(); // 追加: 変化フィールドの値
        public Dictionary<string, string> Result { get; set; } = new();  // 結果の保存先
    }
}
