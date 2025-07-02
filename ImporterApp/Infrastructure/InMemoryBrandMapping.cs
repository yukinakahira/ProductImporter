using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // ブランドIDのマッピング（疑似マスタ）
    public static class InMemoryBrandMapping
    {
        // 連携元ブランドID → ゴールデンブランドID
        public static readonly Dictionary<string, string> BrandMap = new Dictionary<string, string>
        {
            { "BR001", "GoldenBR001" },
            { "BR002", "GoldenBR002" },
            { "BR003", "GoldenBR001" },
            { "BR004", "GoldenBR002" },
            { "BR005", "GoldenBR005" }
        };
        public static readonly Dictionary<string, string> BeforeBrandMap = new Dictionary<string, string>
        {
            { "BR001", "LV" },
            { "BR002", "CHANNEL" },
            { "BR003", "ルイヴィトン" },
            { "BR004", "シャネル" },
        };
    }
}
