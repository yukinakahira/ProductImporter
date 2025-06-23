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
                new() { AttributeId = "WEIGHT", Usage = "バッグ", MappedAttributeId = "WEIGHT変換済" },
                new() { AttributeId = "SIZE_1", Usage = "衣料品", MappedAttributeId = "SHOULDER_WIDTH変換済" },
                new() { AttributeId = "SIZE_2", Usage = "衣料品", MappedAttributeId = "CHEST_WIDTH変換済" },
                new() { AttributeId = "SIZE_3", Usage = "衣料品", MappedAttributeId = "CLOTH_LENGTH変換済" },
                new() { AttributeId = "WEIGHT",  Usage = "衣料品", MappedAttributeId = "FABRIC_WEIGHT変換済" }
            };
        }
    }
}
