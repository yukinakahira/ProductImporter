using System;

namespace ImporterApp.Models
{
    
    /// This model represents the '項目定義マスタ' (Item Definition Master) table.

    
    public class ItemDefinition
    {
        
        /// 項目ID (Item ID) - The unique identifier for the item definition (e.g., 'color').
       
        public string ItemId { get; set; } = string.Empty;

        
        /// 項目キー (Item Key) - A unique key for the item.
        
        public string ItemKey { get; set; } = string.Empty;

        
        /// 項目名称(デフォルト) (Item Name (Default)) - The default display name for the item.
        
        public string DefaultItemName { get; set; } = string.Empty;

        
        /// 表示順 (Display Order) - The order in which this item should be displayed.
        
        public int DisplayOrder { get; set; }

        /// 商材 (Product Material/Merchandise) - The type of product or material this item relates to.
        
        public string? Merchandise { get; set; }

        
        /// 型 (Type) - The type or model.
        
        public string? Type { get; set; }

        
        /// 保存形式管理コード (Storage Format Management Code) - A management code for the storage format.
        
        public string StorageFormatManagementCode { get; set; } = string.Empty;

        
        /// 保存形式コード (Storage Format Code) - The code for the data type (e.g., 'string', 'number').
        
        public string StorageFormatCode { get; set; } = string.Empty;

        
        /// 単位 (Unit) - The unit of measurement (e.g., 'cm', 'kg').
        
        public string? Unit { get; set; }

        
        /// 単複区分管理コード (Singular/Plural Class Management Code) - Management code for singular/plural classification.
        
        public string SingularPluralManagementCode { get; set; } = string.Empty;

        
        /// 単複区分コード (Singular/Plural Class Code) - Code for singular or plural.
        
        public string SingularPluralCode { get; set; } = string.Empty;

        
        /// 必須フラグ (Required Flag) - Indicates if this item is mandatory.
        
        public bool IsRequired { get; set; }

        
        /// 保存対象テーブル (Storage Target Table) - The database table where this item's value is stored.
        
        public string TargetTable { get; set; } = string.Empty;

        
        /// 保存対象カラム名 (Storage Target Column Name) - The database column where this item's value is stored.
        
        public string TargetColumn { get; set; } = string.Empty;

       
        /// 備考 (Remarks) - Optional notes or comments.
       
        public string? Remarks { get; set; }
    }
}