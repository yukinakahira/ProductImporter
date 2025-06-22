namespace ImporterApp.Models
{
    // 履歴情報モデル
    public class ProductHistory
    {
        public string ProductCode { get; set; }= string.Empty;
        public DateTime ChangedAt { get; set; }
        public List<string> ChangedFields { get; set; } = new();
    }
}
