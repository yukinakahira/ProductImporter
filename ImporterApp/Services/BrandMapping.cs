using ImporterApp.Infrastructure;
using ImporterApp.Models;
using System; // For Logger (assuming it's in System or a base namespace)
using System.Collections.Generic;

namespace ImporterApp.Services
{
    // ブランドマッピングサービス
    public class BrandMappingService // クラス名をBrandMappingからBrandMappingServiceに変更し、より責務を明確にしました
    {
        /// <summary>
        /// TempProductのBrandIdをゴールデンブランドIDにマッピングするメソッド
        /// </summary>
        public bool BrandMapping(string brand)
        {
            // ブランドIDマッピング辞書を参照し、存在すればtrue、なければfalseを返す
            var brandMap = InMemoryBrandMapping.BrandMap;
            return brandMap.ContainsKey(brand);
        }
    }
}