namespace ImporterApp.Models
{
    public class GoldenCategory
    {
        /// <summary>
        /// GカテゴリID
        /// </summary>
        public string CategoryId { get; set; } = string.Empty;

        /// <summary>
        /// 親GカテゴリID
        /// </summary>
        public string ParentCategoryId { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 階層レベル
        /// </summary>
        public int HierarchyLevel { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 有効フラグ
        /// </summary>
        public bool IsActive { get; set; }
    }
}
