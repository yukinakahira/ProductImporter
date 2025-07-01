using System;

namespace ImporterApp.Models
{
    
    /// This model represents the 'ユースジルールバインディング' (Usage Rule Binding) table.
    /// It creates a many-to-many relationship between rule usages and rules.
   
    public class UsageRuleBinding
    {
        
        /// ユースジID (Usage ID) - The foreign key for the rule usage context.
        
        public string UsageId { get; set; } = string.Empty;

        
        /// ルールID (Rule ID) - The foreign key for the rule.
        
        public string RuleId { get; set; } = string.Empty;

        
        /// ルール種別 (冗長) (Rule Type (Redundant)) - The type of the rule, likely stored here for convenience.
       
        public string RuleType { get; set; } = string.Empty;
    }
}