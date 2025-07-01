using System;

namespace ImporterApp.Models
{
    
    /// This model represents the 'ルール定義マスタ' (Rule Definition Master) table.
    /// It defines the core properties of a single rule.
    
    public class RuleDefinition
    {
        
        /// ルールID (Rule ID) - The unique identifier for the rule.
        
        public string RuleId { get; set; } = string.Empty;

        
        /// ルール種別 (冗長) (Rule Type (Redundant)) - The type of the rule, likely stored here for convenience.
        
        public string RuleType { get; set; } = string.Empty;

        
        /// 対象項目名 (Target Item Name) - The name of the item that this rule targets.
       
        public string TargetItemName { get; set; } = string.Empty;

        
        /// 結果値 (Result Value) - The value to be used or returned when the rule is successfully applied.
       
        public string ResultValue { get; set; } = string.Empty;

        
        /// 優先度 (Priority) - The execution priority of this rule.
        
        public int Priority { get; set; }

        
        /// 有効フラグ (Enabled Flag) - A flag to indicate if this rule is active.
        
        public bool IsEnabled { get; set; }

        
        /// 備考 (Remarks) - Optional notes or comments about the rule.
        
        public string? Remarks { get; set; }
    }
}