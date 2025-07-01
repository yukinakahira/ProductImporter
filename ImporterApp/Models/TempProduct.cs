namespace ImporterApp.Models;
// 一時商品マスタモデル
public class TempProduct
{
    //ゴールデン商品コード
    public string GProductCode { get; set; } = string.Empty;
    //グループ会社ID
    public string GpCompanyId { get; set; } = string.Empty;
    //連携元商品コード
    public string ProductCode { get; set; } = string.Empty;
    //連携元ブランドID
    public string BrandId { get; set; } = string.Empty;
    //連携元ブランド名
    public string BrandName { get; set; } = string.Empty;
    //商品名
    public string ProductName { get; set; } = string.Empty;
    //連携元カテゴリID
    public string CategoryId { get; set; } = string.Empty;
    //連携元カテゴリ名
    public string CategoryName { get; set; } = string.Empty;
    //商品状態
    public string State { get; set; } = string.Empty;
    //在庫有無
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
    public List<GoldenBrand> goldenBrands { get; set; } = new List<GoldenBrand>();
    public List<GoldenCategory> GoldenCategories { get; set; } = new List<GoldenCategory>();
    public List<ProductHistory> Histories { get; set; } = new List<ProductHistory>();
}
