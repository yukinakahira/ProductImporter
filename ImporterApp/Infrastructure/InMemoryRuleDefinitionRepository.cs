using ImporterApp.Models;
using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    /// <summary>
    /// An in-memory repository to provide test data for the 'RuleDefinition' master.
    /// </summary>
    public class InMemoryRuleDefinitionRepository
    {
        /// <summary>
        /// Gets a predefined list of rule definitions based on the provided spreadsheet data.
        /// </summary>
        /// <returns>A list of RuleDefinition objects.</returns>
        public List<RuleDefinition> GetRuleDefinitions()
        {
            return new List<RuleDefinition>
            {
                // Rule for RKE: Set status to "06" (販売中 - On Sale)
                new()
                {
                    RuleId = "RULE_RKE_STATUS_06",
                    RuleType = "解釈して登録", // Interpret and register
                    TargetItemName = "STATUS",
                    ResultValue = "06",
                    Priority = 20,
                    IsEnabled = true,
                    Remarks = "販売中"
                },
                // Rule for RKE: Set status to "11" (出荷済 - Shipped)
                new()
                {
                    RuleId = "RULE_RKE_STATUS_11",
                    RuleType = "解釈して登録", // Interpret and register
                    TargetItemName = "STATUS",
                    ResultValue = "11",
                    Priority = 20,
                    IsEnabled = true,
                    Remarks = "出荷済"
                },
                // Rule for KM: Set status to "03" (仕入登録済 - Purchase Registered)
                new()
                {
                    RuleId = "RULE_KM_STATUS_03",
                    RuleType = "解釈して登録", // Interpret and register
                    TargetItemName = "STATUS",
                    ResultValue = "03",
                    Priority = 20,
                    IsEnabled = true,
                    Remarks = "仕入登録済"
                },
                // Rule for KM: Map bag height
                new()
                {
                    RuleId = "RULE_KM_BAG_HEI",
                    RuleType = "そのまま登録", // Register as is
                    TargetItemName = "BAG_HEI",
                    ResultValue = "VALUE", // The target column in the EAV table
                    Priority = 50,
                    IsEnabled = true,
                    Remarks = "バッグ縦(mm)"
                },
                // Rule for KM: Map bag width
                new()
                {
                    RuleId = "RULE_KM_BAG_WID",
                    RuleType = "そのまま登録", // Register as is
                    TargetItemName = "BAG_WID",
                    ResultValue = "VALUE",
                    Priority = 50,
                    IsEnabled = true,
                    Remarks = "バッグ横(mm)"
                },
                // Rule for KM: Handle new products
                new()
                {
                    RuleId = "RULE_KM_NEW_PRODUCT",
                    RuleType = "新品商品ロジック", // New Product Logic
                    TargetItemName = "*",
                    ResultValue = "-",
                    Priority = 99,
                    IsEnabled = true,
                    Remarks = "新品用の特別なロジック"
                }
            };
        }
    }
}