using ImporterApp.Models;

namespace ImporterApp.Infrastructure
{
    public class InMemoryMeaningRuleRepository
    {
        public List<AttributeMeaningRule> GetMeaningRules()
        {
            return new List<AttributeMeaningRule>
            {
                new() { AttributeId = "SIZE_1", Usage = "バッグ", MappedAttributeId = "SIZE_VERTICAL変換済" },
                new() { AttributeId = "SIZE_2", Usage = "バッグ", MappedAttributeId = "SIZE_HORIZONTAL変換済" },
                new() { AttributeId = "SIZE_3", Usage = "バッグ", MappedAttributeId = "SIZE_DEPTH変換済" },
                new() { AttributeId = "SIZE_1", Usage = "ジュエリー", MappedAttributeId = "RING_SIZE変換済" },
                new() { AttributeId = "WEIGHT", Usage = "バッグ", MappedAttributeId = "WEIGHT変換済" }
            };
        }
    }
}
