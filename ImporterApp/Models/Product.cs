namespace ImporterApp.Models;
// 商品マスタモデル
public class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string BrandId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    public string SlipType { get; set; } = string.Empty;// 伝票区分 (4列目)
    public string ProductStatus { get; set; } = string.Empty;// 商品状態 (2列目)
    public string Category { get; set; } = string.Empty;// カテゴリ (3列目)
    public string SubWarehouseCode { get; set; } = string.Empty;// 副倉庫コード（13列目）
    public string NewProductCategory { get; set; } = string.Empty;// 新品商品区分（10列目）
}
