namespace ImporterApp.Models;
// 商品マスタモデル
public class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string BrandId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zaikoumu { get; set; } = string.Empty;
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
}
