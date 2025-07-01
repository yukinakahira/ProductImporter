using System;

namespace ImporterApp.Models
{
   
    /// This model represents the '条件付き項目適用マスタ' (Conditional Item Application Master) table.
    
    public class ConditionalItemApplication
    {
       
        /// 条件適用ID (Condition Application ID) - The unique identifier for this rule application.
        
        public string ConditionApplicationId { get; set; } = string.Empty;

       
        /// 項目ID (Item ID) - The ID of the item this condition applies to (e.g., 'color', 'size').
        
        public string ItemId { get; set; } = string.Empty;
        /// ルールID (Rule ID) - The ID of the rule to be applied. 
        public string RuleId { get; set; } = string.Empty;

        
        /// 優先度 (Priority) - The execution priority of this rule. Lower numbers are typically higher priority.
        
        public int Priority { get; set; }

        
        /// 有効フラグ (Enabled Flag) - A flag to indicate if this rule application is active.
        
        public bool IsEnabled { get; set; }

       
        /// 備考 (Remarks) - Optional notes or comments.
        
        public string? Remarks { get; set; }
    }
}