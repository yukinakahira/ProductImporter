using System;

namespace ImporterApp.Models
{
    // This model represents the '商品画像マスタ' (Product Image Master) table [cite: 1]
    public class ProductImage
    {
        // G商品コード (G Product Code) [cite: 1]
        public string GProductCode { get; set; } = string.Empty;

        // 画像ID (Image ID) [cite: 1]
        public string ImageId { get; set; } = string.Empty;

        // 画像パス (Image Path) [cite: 1]
        public string ImagePath { get; set; } = string.Empty;

        // 画像種別管理コード (Image Type Management Code) [cite: 1]
        public string ImageTypeManagementCode { get; set; } = string.Empty;

        // 画像種別コード (Image Type Code), e.g., Main Image, Sub Image, etc. [cite: 1]
        public string ImageTypeCode { get; set; } = string.Empty;

        // 表示順 (Display Order) [cite: 1]
        public int DisplayOrder { get; set; }

        // 撮影日 (Shooting Date) [cite: 1]
        public DateTime? ShootingDate { get; set; }
    }
}