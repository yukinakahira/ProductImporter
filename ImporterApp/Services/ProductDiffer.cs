using ImporterApp.Models;

namespace ImporterApp.Services
{
    // 商品情報差分判定ロジック
    public static class ProductDiffer
    {
        public static List<string> GetChangedFields(Product oldData, Product newData)
        {
            var changes = new List<string>();

            if (oldData.BrandId != newData.BrandId)
                changes.Add("BrandId");

            if (oldData.ProductName != newData.ProductName)
                changes.Add("ProductName");

            // 今後他の項目（Size1など）が増えたらここに追加

            return changes;
        }
    }
}
