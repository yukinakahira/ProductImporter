namespace ImporterApp.Models;
// 商品マスタモデル
public class Product
{
    public string GProductCode { get; set; } = string.Empty;
    public string LinkedProductCode { get; set; } = string.Empty;
    public string GpCompanyId { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string BrandId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zaikoumu { get; set; } = string.Empty;
    //新增
    public string ProductStatusManageCode { get; set; } = string.Empty;
    public string ProductStatusCode { get; set; } = string.Empty;
    public string StockAvailability { get; set; } = string.Empty;
    public string SaleAvailability { get; set; } = string.Empty;
    public string AssessmentPrice { get; set; } = string.Empty;
    public string PurchasePrice { get; set; } = string.Empty;
    public string DisplayPrice { get; set; } = string.Empty;
    public string SalesPrice { get; set; } = string.Empty;
    public string StoreId { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string ConsignmentGpCompanyId { get; set; } = string.Empty;
    public string ConsignedProductCode { get; set; } = string.Empty;
    public string UpdateType { get; set; } = string.Empty;
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
}
