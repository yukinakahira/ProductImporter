using ImporterApp.Models;
using System.Linq;

namespace ImporterApp.Services
{
    public static class AttributeMeaningMapper
    {
        public static string Map(string attributeId, string usage, List<AttributeMeaningRule> rules)
        {
            return rules
                .FirstOrDefault(r => r.AttributeId == attributeId && r.Usage == usage)
                ?.MappedAttributeId ?? attributeId; // 該当がない場合は元のIDを使用
        }
    }
}
