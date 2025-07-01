using ImporterApp.Models;
using System.Collections.Generic;

namespace ImporterApp.Services
{
    // 商品情報差分判定ロジック
    public static class BrandMapping
    {
        // 差分フィールド名と新値の辞書を返す
        public static Dictionary<string, string> GetChangedFieldValues(Product oldData, Product newData)
        {
            var changes = new Dictionary<string, string>();

            if (oldData.BrandId != newData.BrandId)
                changes["BrandId"] = newData.BrandId;

            if (oldData.ProductName != newData.ProductName)
                changes["ProductName"] = newData.ProductName;

            // 今後他の項目（Size1など）が増えたらここに追加

            return changes;
        }

        // 既存のリスト型も残す
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
