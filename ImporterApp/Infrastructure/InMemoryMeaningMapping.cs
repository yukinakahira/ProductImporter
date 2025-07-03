using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // ゴールデンブランドID→ブランド名の疑似マスタ
    public static class InMemoryMeaningMapping
    {
        //ブランドIDからブランド名を取得するため
        public static readonly Dictionary<string, string> GoldenBrandNameMap = new Dictionary<string, string>
        {
            { "GoldenBR001", "Louisvuitton" },
            { "GoldenBR002", "Chanel" },
            { "GoldenBR003", "Hermès" },
            { "GoldenBR004", "Gucci" },
            { "GoldenBR005", "Cartier" }
            // 必要に応じて追加
        };
        // カテゴリIDからカテゴリ名を取得するため
        public static readonly Dictionary<string, string> GoldenCategoryNameMap = new Dictionary<string, string>
        {
            { "GoldenC001", "バッグ" },
            { "GoldenC002", "宝石" },
            { "GoldenC003", "時計" },
            { "GoldenC004", "衣料品" }
            // 必要に応じて追加
        };
    }
}
