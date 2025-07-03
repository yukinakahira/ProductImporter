using ImporterApp.Infrastructure;   

namespace ImporterApp.Models
{
    /// <summary>
    /// Represents a product entry pending approval due to missing required fields.
    /// </summary>
    public class ApprovalPending
    {
        /// <summary>
        /// UUIDなどの一意な識別子
        /// </summary>
        public string PendingId { get; set; } = InMemoryProductRepository.GetNextPendingId();

        /// <summary>
        /// 対象種別（例：BRAND / CATEGORY）
        /// </summary>
        public string PendingType { get; set; } = string.Empty;

        /// <summary>
        /// 商品マスタ上の項目ID（例：BrandId）
        /// </summary>
        public string OriginalId { get; set; } = string.Empty;

        /// <summary>
        /// 商品マスタ上の項目名（例：BrandName）
        /// </summary>
        public string OriginalName { get; set; } = string.Empty;

        /// <summary>
        /// 用途ID（何で承認フローに入ったか識別）
        /// </summary>
        public string UsageId { get; set; } = string.Empty;

        /// <summary>
        /// ステータス（例：PENDING, APPROVED, REJECTED）
        /// </summary>
        public string State { get; set; } = "PENDING";

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 承認日時（NULLなら未承認）
        /// </summary>
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// 承認者ユーザーID
        /// </summary>
        public string ApprovedBy { get; set; } = string.Empty;

        /// <summary>
        /// 備考・対応メモ
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// The original CSV data for reference.
        /// </summary>
        public Dictionary<string, string> CsvData { get; set; } = new();

        /// <summary>
        /// Indicates whether this entry has been reviewed/checked.
        /// </summary>
        public bool IsChecked { get; set; } = false;

        /// <summary>
        /// ブランドIDがない場合のフラグ
        /// </summary>
        public bool IsBrandIdMissing => string.IsNullOrEmpty(OriginalId) && PendingType == "BRAND";

        /// <summary>
        /// ブランド名がない場合のフラグ
        /// </summary>
        public bool IsBrandNameMissing => string.IsNullOrEmpty(OriginalName) && PendingType == "BRAND";
    }
}