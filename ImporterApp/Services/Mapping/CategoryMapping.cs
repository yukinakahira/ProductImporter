using ImporterApp.Infrastructure;
using ImporterApp.Models;
using System; // For Logger (assuming it's in System or a base namespace)
using System.Collections.Generic;

namespace ImporterApp.Services.Mapping
{
    // ブランドマッピングサービス
    public class CategoryMappingService // クラス名をBrandMappingからBrandMappingServiceに変更し、より責務を明確にしました
    {
        /// <summary>
        /// TempProductのBrandIdをゴールデンブランドIDにマッピングするメソッド
        /// </summary>
        public bool CategoryMapping(string categoryId)
        {
            // カテゴリIDマッピング辞書を参照し、存在すればtrue、なければfalseを返す
            var categoryMap = InMemoryCategoryMapping.CategoryMap;
            return categoryMap.ContainsKey(categoryId);
        }
    }
}