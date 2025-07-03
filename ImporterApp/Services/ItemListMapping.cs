using ImporterApp.Infrastructure;
using ImporterApp.Models;
using System.Collections.Generic;

namespace ImporterApp.Services
{
    public class ItemListMappingService
    {
        /// <summary>
        /// 項目リスト（例：色、サイズ等）をゴールデン項目リストIDにマッピングする汎用メソッド
        public bool MapItemList(string itemId, string itemListId)
        {
            return InMemoryItemLisytMapping.ItemListMap.ContainsKey((itemId, itemListId));
        }
    }
}

//这里的逻辑几乎和以上的BrandMappingService一样
//唯一的不同是，传参的时候，会传入一个项目ID，根据这个项目ID会去特定一个对象项目，比如颜色
//这样做的话此方法可以复用到其他的项目上，比如尺寸、材质等
//按照这个项目ID如18，我可以去特定一个项目ListID也就是COLOR001，COLOR001代表红色
//所以这里做的mapping是将所有COLOR001的项目ID都映射到GoldenCOLOR001
//运用Infrastructure/InMemoryItemListMapping.cs当中的设定
//当出现为被设定的颜色，如白色的时候，把数据存入Models/ApprovalPending.cs