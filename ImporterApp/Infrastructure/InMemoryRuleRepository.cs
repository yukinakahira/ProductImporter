using ImporterApp.Models;

namespace ImporterApp.Infrastructure
{
    // ステージングカラム→商品マスタ項目の仮想ルールモック（疑似データ）
    // ファイル取込ルールマスタ×ファイル取込ルール詳細マスタ×項目定義マスタ（DBより取得）
    // ルール条件テーブル×条件付き項目適用マスタの扱いは考え中
    public class InMemoryRuleRepository
    {
        public List<FileImportRuleDetail> GetRules(string userScenarioId)
        {
            // ユースジIDに応じたルールを返す
            return new List<FileImportRuleDetail>
            {
                new() { ColumnName = "COL_0", AttributeId = "PRODUCT_CODE", SaveTable = "PRODUCT_MST", SaveColumn = "PRODUCT_CODE" },
                new() { ColumnName = "COL_1", AttributeId = "BRAND_ID", SaveTable = "PRODUCT_MST", SaveColumn = "BRAND_ID" },
                new() { ColumnName = "COL_2", AttributeId = "PRODUCT_NAME", SaveTable = "PRODUCT_MST", SaveColumn = "PRODUCT_NAME" },
                new() { ColumnName = "COL_3", AttributeId = "SIZE_1", SaveTable = "PRODUCT_EAV", SaveColumn = "VALUE" },
                new() { ColumnName = "COL_4", AttributeId = "SIZE_2", SaveTable = "PRODUCT_EAV", SaveColumn = "VALUE" },
                new() { ColumnName = "COL_5", AttributeId = "SIZE_3", SaveTable = "PRODUCT_EAV", SaveColumn = "VALUE" },
                new() { ColumnName = "COL_6", AttributeId = "WEIGHT", SaveTable = "PRODUCT_EAV", SaveColumn = "VALUE" }
            };
        }
    }
}
