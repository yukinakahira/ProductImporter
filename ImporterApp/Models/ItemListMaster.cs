using System;

namespace ImporterApp.Models
{
    
    /// This model represents the '項目リストマスタ' (Item List Master) table.
    /// It manages lists of items, often used for dropdowns or predefined value sets.
   
    public class ItemList
    {
        
        /// 項目ID (Item ID) - The ID of the item this list is associated with.
       
        public string ItemId { get; set; } = string.Empty;

        
        /// GP会社ID (GP Company ID) - The ID of the company this list belongs to.
        
        public string GpCompanyId { get; set; } = string.Empty;

       
        /// 項目リストID (Item List ID) - The unique identifier for this item list.
        
        public string ItemListId { get; set; } = string.Empty;

        
        /// 連携元リストID (Source List ID) - The ID of this list in the original source system.
        
        public string? SourceListId { get; set; }

        
        /// 項目リスト名称(デフォルト) (Item List Name (Default)) - The default display name of the list.
       
        public string DefaultListName { get; set; } = string.Empty;

        
        /// 統合フラグ (Integration Flag) - A flag indicating if the list is integrated.
        
        public bool IsIntegrated { get; set; }

        
        /// 表示順 (Display Order) - The order in which this list should be displayed.
        
        public int DisplayOrder { get; set; }
    }
}