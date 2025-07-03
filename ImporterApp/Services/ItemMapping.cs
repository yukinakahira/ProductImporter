
namespace Services  
{
    using System.Collections.Generic;
    using System.Linq;
    using ImporterApp.Models;

    public class ItemMapping
    {
        private readonly Dictionary<string, string> _itemMappings;

        public ItemMapping()
        {
            _itemMappings = new Dictionary<string, string>
            {
                { "Item1", "MappedItem1" },
                { "Item2", "MappedItem2" },
                { "Item3", "MappedItem3" }
            };
        }

        public string GetMappedItem(string itemName)
        {
            return _itemMappings.TryGetValue(itemName, out var mappedItem) ? mappedItem : null;
        }
    }
}