using ImporterApp.Models;
using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    /// <summary>
    /// An in-memory repository to provide test data for the 'RuleUsage' master.
    /// </summary>
    public class InMemoryRuleUsageRepository
    {
        /// <summary>
        /// Gets a predefined list of rule usage contexts.
        /// </summary>
        /// <returns>A list of RuleUsage objects.</returns>
        public List<RuleUsage> GetRuleUsages()
        {
            return new List<RuleUsage>
            {
                new()
                {
                    UsageId = "RULE_RKE_00",
                    UsageName = "RKE Rule 00",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'RKE'",
                    WhereClause = "Type == 'Stock'",
                    Remarks = "RKE Stock Rule"
                },
                new()
                {
                    UsageId = "RULE_RKE_01",
                    UsageName = "RKE Rule 01",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'RKE'",
                    WhereClause = "Type == 'Sales'",
                    Remarks = "RKE Sales Rule"
                },
                new()
                {
                    UsageId = "RULE_RKE_02",
                    UsageName = "RKE Rule 02",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'RKE'",
                    WhereClause = "",
                    Remarks = "RKE General Rule"
                },
                new()
                {
                    UsageId = "RULE_KM_01",
                    UsageName = "KM Rule 01",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'KM'",
                    WhereClause = "",
                    Remarks = "KM Base Rule"
                },
                 new()
                {
                    UsageId = "RULE_KM_02",
                    UsageName = "KM Rule 02",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'KM'",
                    WhereClause = "Category == 'Bags'",
                    Remarks = "KM Bag Category Rule"
                },
                new()
                {
                    UsageId = "RULE_KM_03",
                    UsageName = "KM Rule 03",
                    RuleType = "商品データ取り込み",
                    WhenCondition = "Source == 'KM'",
                    WhereClause = "Category == 'Jewelry'",
                    Remarks = "KM Jewelry Category Rule"
                }
            };
        }
    }
}