using ImporterApp.Models;

namespace ImporterApp.Services.Validation
{
    // チェック処理
    public class ProductValidator
    {
        public static List<string> Validate(Product product)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                errors.Add("ProductCode is required.");
            }

            if (string.IsNullOrWhiteSpace(product.BrandId))
            {
                errors.Add("BrandId is required.");
            }

            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                errors.Add("ProductName is required.");
            }

            // 数値項目（例: サイズ）などの型チェックがあればここに追加
            // 例:
            // if (!int.TryParse(product.Size1, out _))
            // {
            //     errors.Add("Size1 must be an integer.");
            // }

            return errors;
        }
    }
}
