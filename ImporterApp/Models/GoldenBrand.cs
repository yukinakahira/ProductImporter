namespace ImporterApp.Models
{
    /// <summary>
    /// Gブランドマスタモデル
    /// </summary>
    public class GoldenBrand
    {
        /// <summary>
        /// ゴールデンブランドID
        /// </summary>
        public string GBrandId { get; set; } = string.Empty;

        /// <summary>
        /// ブランド名
        /// </summary>
        public string BrandName { get; set; } = string.Empty;

        /// <summary>
        /// 言語コード
        /// </summary>
        public string LanguageCode { get; set; } = string.Empty;
    }
}