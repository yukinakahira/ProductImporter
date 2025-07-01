using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // ゴールデンブランドID→ブランド名の疑似マスタ
    public static class InMemoryMeaningMapping
    {
        public static readonly Dictionary<string, string> GoldenBrandNameMap = new Dictionary<string, string>
        {
            { "GoldenBR001", "ルイ・ヴィトン" },
            { "GoldenBR002", "Chanel" },
            { "GoldenBR003", "Hermès" },
            { "GoldenBR004", "Gucci" },
            { "GoldenBR005", "Cartier" }
            // 必要に応じて追加
        };
    }
}
