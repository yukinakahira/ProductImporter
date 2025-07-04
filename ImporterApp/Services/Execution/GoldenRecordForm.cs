using ImporterApp.Models;

namespace ImporterApp.Services.Execution
{
    public static class GoldenRecordForm
    {
        /// <summary>
        /// Productオブジェクトを受け取り、そのProductCodeの前に"Golden"を付与してゴールデン商品コードを生成する
        /// </summary>
        /// <param name="product">商品オブジェクト</param>
        /// <returns>ゴールデン商品コード</returns>
        public static string GenerateGoldenProductCode(Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.ProductCode))
                return string.Empty;
            return $"Golden{product.ProductCode}";
        }
    }
}