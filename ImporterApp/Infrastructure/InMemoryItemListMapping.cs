using System.Collections.Generic;

namespace ImporterApp.Infrastructure
{
    // カテゴリIDのマッピング（疑似マスタ）
    public static class InMemoryItemLisytMapping
    {
        // 連携元項目リストID → ゴールデン項目リストID
        // カラーマッピング
        //这里的修正是 第一个数值代表用途，比如COLOR，MATERIAL等
        //第二三个用作mapping 意思是連携元項目リストID 等于 ゴールデン項目リストID
        //请按照次含义修正代码
        // 用途+元ID でゴールデンIDを引く (用途, 元ID) => ゴールデンID
        public static readonly Dictionary<(string Usage, string SourceId), string> ItemListMap = new Dictionary<(string, string), string>
        {
            { ("COLOR_ID", "COLOR001"), "GoldenCOLOR001" },
            { ("COLOR_ID", "COLOR002"), "GoldenCOLOR002" },
            { ("COLOR_ID", "COLOR003"), "GoldenCOLOR003" },
            { ("COLOR_ID", "COLOR004"), "GoldenCOLOR004" },
            { ("COLOR_ID", "RKE_Cl_001"), "GoldenCOLOR001" },
            { ("COLOR_ID", "RKE_Cl_002"), "GoldenCOLOR002" },
            // MATERIAL等も同様に追加可能
            { ("MATERIAL_ID", "MAT001"), "GoldenMAT001" },
            { ("MATERIAL_ID", "MAT002"), "GoldenMAT002" },
            { ("MATERIAL_ID", "MAT003"), "GoldenMAT003" },
            { ("MATERIAL_ID", "RKE_Mat_001"), "GoldenMAT001" },
            { ("MATERIAL_ID", "RKE_Mat_002"), "GoldenMAT002" },
        };
        
    }
}
