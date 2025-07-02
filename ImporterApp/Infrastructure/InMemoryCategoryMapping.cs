using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // カテゴリIDのマッピング（疑似マスタ）
    public static class InMemoryCategoryMapping
    {
        // 連携元カテゴリID → ゴールデンカテゴリID
        public static readonly Dictionary<string, string> CategoryMap = new Dictionary<string, string>
        {
            { "C001", "GoldenC001" }, // バッグ
            { "C002", "GoldenC002" }, // 宝石
            { "C003", "GoldenC003" },// 時計
            { "C004", "GoldenC004" }, // 衣料品
        };
        
    }
}
