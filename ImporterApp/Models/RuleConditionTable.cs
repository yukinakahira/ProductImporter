using System;

namespace ImporterApp.Models
{
    
    /// This model represents the 'ルール条件テーブル' (Rule Condition Table).
    /// It defines a single condition that is part of a larger rule.
    
    public class RuleConditionTable
    {
        
        /// ルールID (Rule ID) - The foreign key linking this condition to a specific rule.
       
        public string RuleId { get; set; } = string.Empty;

        
        /// 条件SEQ (Condition SEQ) - The sequence number of this condition within the rule.
        
        public int ConditionSequence { get; set; }

        
        /// 項目ID (Item ID) - The ID of the item to be evaluated in this condition.
       
        public string ItemId { get; set; } = string.Empty;

        
        /// 演算子 (Operator) - The comparison operator to use (e.g., '=', '>', 'LIKE').
        
        public string Operator { get; set; } = string.Empty;

        
        /// 比較値 (Comparison Value) - The value to compare the item against.
        
        public string ComparisonValue { get; set; } = string.Empty;

       
        /// 論理 (Logic) - The logical operator (e.g., 'AND', 'OR') connecting this condition to the next one.
    
        public string? Logic { get; set; }
    }
}