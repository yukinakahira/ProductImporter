namespace ImporterApp.Models
{
    // 履歴情報モデル
    public class ProductHistory
    {
        public string HistoryId { get; set; } = string.Empty;

        /// <summary>
        /// 商品コード
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 店舗ID
        /// </summary>
        public string StoreId { get; set; } = string.Empty;

        /// <summary>
        /// 在庫数
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 在庫状態管理コード
        /// </summary>
        public string StockStatusManagementCode { get; set; } = string.Empty;

        /// <summary>
        /// 在庫状態コード
        /// </summary>
        public string StockStatusCode { get; set; } = string.Empty;

        /// <summary>
        /// 入庫日
        /// </summary>
        public DateTime? ReceivingDate { get; set; }

        /// <summary>
        /// 出庫日
        /// </summary>
        public DateTime? ShippingDate { get; set; }

        /// <summary>
        /// 移動元店舗ID
        /// </summary>
        public string SourceStoreId { get; set; } = string.Empty;

        /// <summary>
        /// 移動先店舗ID
        /// </summary>
        public string DestinationStoreId { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// 変更日時
        /// </summary>
        public DateTime ChangedAt { get; set; }

        /// <summary>
        /// 変更されたフィールドリスト
        /// </summary>
        public List<string> ChangedFields { get; set; } = new();

        /// <summary>
        /// 変更フィールドの値
        /// </summary>
        public Dictionary<string, string> ChangedFieldValues { get; set; } = new();
    }
}
