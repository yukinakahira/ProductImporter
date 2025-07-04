namespace ImporterApp.Models;
// 商品マスタモデル
public class Product
{
    // ゴールデン商品コード
    public string GProductCode { get; set; } = string.Empty;
    // グループ会社ID
    public string GpCompanyId { get; set; } = string.Empty;
    // 連携元商品コード
    public string ProductCode { get; set; } = string.Empty;
    // 連携元ブランドID
    public string BrandId { get; set; } = string.Empty;
    // 連携元ブランド名
    public string BrandName { get; set; } = string.Empty;
    // 商品名
    public string ProductName { get; set; } = string.Empty;
    // 連携元カテゴリID
    public string CategoryId { get; set; } = string.Empty;
    // 連携元カテゴリ名
    public string CategoryName { get; set; } = string.Empty;
    // 商品状態コード（例：販売中、在庫切れ等）
    public string ProductStatusCode { get; set; } = string.Empty; 
    // 在庫有無
    public string StockAvailability { get; set; } = string.Empty;
    // 販売可否
    public string SaleAvailability { get; set; } = string.Empty;
    // 店舗ID
    public string StoreId { get; set; } = string.Empty;
    // 店舗名
    public string StoreName { get; set; } = string.Empty;
    // 仕入価格
    public string PurchasePrice { get; set; } = string.Empty;
    // 表示価格
    public string DisplayPrice { get; set; } = string.Empty;
    // 販売価格
    public string SalesPrice { get; set; } = string.Empty;
    // 委託元グループ会社ID
    public string ConsignmentGpCompanyId { get; set; } = string.Empty;
    // 委託商品コード
    public string ConsignedProductCode { get; set; } = string.Empty;
    // 担当者名
    public string StaffName { get; set; } = string.Empty;
    // 更新種別
    public string UpdateType { get; set; } = string.Empty;
    // 新品区分（新品/中古など）
    public string NewItemType { get; set; } = string.Empty;
    // 数量
    public int Quantity { get; set; } = 0;
    // 仕入先区分
    public string SupplierType { get; set; } = string.Empty;
    // 販売先区分
    public string SalesDestinationType { get; set; } = string.Empty;
    // 販売先名
    public string SalesDestinationName { get; set; } = string.Empty;
    // 査定価格
    public decimal AssessmentPrice { get; set; } = 0;
    // 更新日
    public string UpdateDate { get; set; } = string.Empty;
    // 更新時
    public string UpdateTime { get; set; } = string.Empty;
    // EAV属性リスト
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();


}
