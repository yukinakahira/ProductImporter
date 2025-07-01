using System;

namespace ImporterApp.Models
{
    
    /// This model represents the 'ルール使用マスタ' (Rule Usage Master) table.
    /// It defines the usage context for various rules.
    
    public class RuleUsage
    {
        
        /// ユースジID (Usage ID) - The unique identifier for the rule usage context.
        
        public string UsageId { get; set; } = string.Empty;

        
        /// ユースジ名 (Usage Name) - The name of the usage context.
       
        public string UsageName { get; set; } = string.Empty;

        
        /// ルール種別 (冗長) (Rule Type (Redundant)) - The type of rule, possibly stored here for convenience.
        
        public string RuleType { get; set; } = string.Empty;

        
        /// WHEN - The WHEN condition clause for the rule's execution.
        
        public string WhenCondition { get; set; } = string.Empty;

        
        /// WHERE - The WHERE filter clause for the rule's execution.
        
        public string WhereClause { get; set; } = string.Empty;

        
        /// 備考 (Remarks) - Optional notes or comments.
        
        public string? Remarks { get; set; }
    }
}