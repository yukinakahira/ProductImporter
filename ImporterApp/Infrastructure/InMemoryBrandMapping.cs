using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // ブランドIDのマッピング（疑似マスタ）
    public static class InMemoryBrandMapping
    {
        // 連携元ブランドID → ゴールデンブランドID
        public static readonly Dictionary<string, string> BrandMap = new Dictionary<string, string>
        {
            { "KM_BR_001", "GoldenBR001" },
            { "KM_BR_002", "GoldenBR002" },
            { "KM_BR_003", "GoldenBR003" },
            { "KM_BR_004", "GoldenBR004" },
            { "KM_BR_005", "GoldenBR005" },
            { "RKE_BR_001", "GoldenBR001" },
            { "RKE_BR_002", "GoldenBR002" },
            { "RKE_BR_003", "GoldenBR003" },
            { "RKE_BR_004", "GoldenBR004" },
            { "RKE_BR_005", "GoldenBR005" }
        };
    }
}
