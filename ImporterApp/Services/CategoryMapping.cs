using ImporterApp.Infrastructure;
using ImporterApp.Models;
<<<<<<< HEAD
using System;
=======
using System; // For Logger (assuming it's in System or a base namespace)
>>>>>>> 7dca1ab7713c3d1d4dc14e785b9636cbbaae4ec5
using System.Collections.Generic;

namespace ImporterApp.Services
{
<<<<<<< HEAD
    // カテゴリマッピングサービス
    public class CategoryMappingService
    {
        /// <summary>
        /// TempProductのCategoryIdをゴールデンカテゴリIDにマッピングするメソッド
        /// </summary>
        public void MapGoldenCategoryId(TempProduct tempProduct, bool isMappingEnabled, List<ApprovalPending> approvalPendings)
        {
            var categoryMap = InMemoryCategoryMapping.CategoryMap;

            // カテゴリIDまたはカテゴリ名が存在しない場合は承認待ちリストに追加
            if (string.IsNullOrEmpty(tempProduct.CategoryId) || string.IsNullOrEmpty(tempProduct.CategoryName))
            {
                Console.WriteLine($"[CategoryMapping] TempProductのCategoryIdまたはCategoryNameが不足しています (ProductCode: {tempProduct.ProductCode}).");
                approvalPendings.Add(new ApprovalPending
                {
                    PendingType = "CATEGORY_MISSING_DATA",
                    OriginalId = tempProduct.CategoryId ?? string.Empty,
                    OriginalName = tempProduct.CategoryName ?? string.Empty,
                    UsageId = tempProduct.ProductCode,
                    Remarks = "カテゴリIDまたはカテゴリ名が不足しています",
                    // CsvData = new Dictionary<string, string>
                    // {
                    //     { "TempProductCode", tempProduct.ProductCode ?? "N/A" },
                    //     { "CategoryId", tempProduct.CategoryId ?? "N/A" },
                    //     { "CategoryName", tempProduct.CategoryName ?? "N/A" }
                    // }
                });
                return; // 処理を終了
            }

            if (isMappingEnabled)
            {
                if (categoryMap.TryGetValue(tempProduct.CategoryId, out var goldenCategoryId))
                {
                    // マッピング成功：TempProductのGoldenCategoryIdプロパティを更新
                    Console.WriteLine($"[CategoryMapping] TempProductのCategoryId({tempProduct.CategoryId})をGoldenCategoryId({goldenCategoryId})にマッピングしました。");
                    tempProduct.GoldenCategoryId = goldenCategoryId;
                }
                else
                {
                    // マッピング失敗：承認待ちリストに追加
                    Console.WriteLine($"[CategoryMapping] TempProductのCategoryId({tempProduct.CategoryId})はマッピングできませんでした。");
                    approvalPendings.Add(new ApprovalPending
                    {
                        PendingType = "CATEGORY",
                        OriginalId = tempProduct.CategoryId,
                        OriginalName = tempProduct.CategoryName,
                        UsageId = tempProduct.ProductCode,
                        Remarks = "カテゴリIDマッピングなし",
                        // CsvData = new Dictionary<string, string>
                        // {
                        //     { "TempProductCode", tempProduct.ProductCode ?? "N/A" },
                        //     { "CategoryId", tempProduct.CategoryId },
                        //     { "CategoryName", tempProduct.CategoryName }
                        // }
                    });
                }
            }
            else
            {
                // マッピングが無効な場合：承認待ちリストに追加
                Console.WriteLine($"[CategoryMapping] isMappingEnabled=falseのため、TempProductのCategoryId({tempProduct.CategoryId})はマッピングしませんでした。");
                approvalPendings.Add(new ApprovalPending
                {
                    PendingType = "CATEGORY",
                    OriginalId = tempProduct.CategoryId,
                    OriginalName = tempProduct.CategoryName,
                    UsageId = tempProduct.ProductCode,
                    Remarks = "isMappingEnabled=falseのためマッピングせず",
                    // CsvData = new Dictionary<string, string>
                    // {
                    //     { "TempProductCode", tempProduct.ProductCode ?? "N/A" },
                    //     { "CategoryId", tempProduct.CategoryId },
                    //     { "CategoryName", tempProduct.CategoryName }
                    // }
                });
            }
        }
    }
}
    // // テスト実行用のクラス
    // public class CategoryMappingTest
    // {
    //     public static void RunTest()
    //     {
    //         var approvalPendings = new List<ApprovalPending>();
    //         var categoryMappingService = new CategoryMappingService();

    //         // 1. マッピング成功テスト (C001)
    //         var tempProduct1 = new TempProduct
    //         {
    //             ProductCode = "TEST001",
    //             CategoryId = "C001",
    //             CategoryName = "バッグ"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct1, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct1.ProductCode}, GoldenCategoryId={tempProduct1.GoldenCategoryId}\n");

    //         // 2. マッピング失敗テスト (UNKNOWN)
    //         var tempProduct2 = new TempProduct
    //         {
    //             ProductCode = "TEST002",
    //             CategoryId = "UNKNOWN",
    //             CategoryName = "UNKNOWN"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct2, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct2.ProductCode}, GoldenCategoryId={tempProduct2.GoldenCategoryId}\n");

    //         // 3. マッピング成功テスト (C003)
    //         var tempProduct3 = new TempProduct
    //         {
    //             ProductCode = "TEST003",
    //             CategoryId = "C003",
    //             CategoryName = "時計"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct3, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct3.ProductCode}, GoldenCategoryId={tempProduct3.GoldenCategoryId}\n");

    //         // 4. マッピング成功テスト (C002)
    //         var tempProduct4 = new TempProduct
    //         {
    //             ProductCode = "TEST004",
    //             CategoryId = "C002",
    //             CategoryName = "宝石"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct4, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct4.ProductCode}, GoldenCategoryId={tempProduct4.GoldenCategoryId}\n");

    //         // 5. マッピング成功テスト (C004)
    //         var tempProduct5 = new TempProduct
    //         {
    //             ProductCode = "TEST005",
    //             CategoryId = "C004",
    //             CategoryName = "衣料品"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct5, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct5.ProductCode}, GoldenCategoryId={tempProduct5.GoldenCategoryId}\n");

    //         // 6. カテゴリIDが不足しているテスト
    //         var tempProduct6 = new TempProduct
    //         {
    //             ProductCode = "TEST006",
    //             CategoryId = "",
    //             CategoryName = "Missing ID Category"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct6, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct6.ProductCode}, GoldenCategoryId={tempProduct6.GoldenCategoryId}\n");

    //         // 7. カテゴリ名が不足しているテスト
    //         var tempProduct7 = new TempProduct
    //         {
    //             ProductCode = "TEST007",
    //             CategoryId = "C999",
    //             CategoryName = ""
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct7, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct7.ProductCode}, GoldenCategoryId={tempProduct7.GoldenCategoryId}\n");

    //         // 8. isMappingEnabled=falseのテスト
    //         var tempProduct8 = new TempProduct
    //         {
    //             ProductCode = "TEST008",
    //             CategoryId = "C001",
    //             CategoryName = "バッグ"
    //         };
    //         categoryMappingService.MapGoldenCategoryId(tempProduct8, false, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct8.ProductCode}, GoldenCategoryId={tempProduct8.GoldenCategoryId}\n");

    //         // 承認待ちリストの結果出力
    //         Console.WriteLine("--- 承認待ちリスト ---");
    //         if (approvalPendings.Count == 0)
    //         {
    //             Console.WriteLine("承認待ちアイテムはありません。");
    //         }
    //         else
    //         {
    //             foreach (var pending in approvalPendings)
    //             {
    //                 Console.WriteLine($"[Test] ApprovalPending: Type={pending.PendingType}, ID={pending.OriginalId}, Name={pending.OriginalName}, Remarks={pending.Remarks}");
    //             }
    //         }
    //     }
    // }

    // メインのエントリーポイント
    // class CategoryMappingTestEntry
    // {
    //     static void Main()
    //     {
    //         CategoryMappingTest.RunTest();
    //     }
    // }
// }
=======
    // ブランドマッピングサービス
    public class CategoryMappingService // クラス名をBrandMappingからBrandMappingServiceに変更し、より責務を明確にしました
    {
        /// <summary>
        /// TempProductのBrandIdをゴールデンブランドIDにマッピングするメソッド
        /// </summary>
        public bool CategoryMapping(string brand)
        {
            // カテゴリIDマッピング辞書を参照し、存在すればtrue、なければfalseを返す
            var categoryMap = InMemoryCategoryMapping.CategoryMap;
            return categoryMap.ContainsKey(brand);
        }
    }
}
>>>>>>> 7dca1ab7713c3d1d4dc14e785b9636cbbaae4ec5
