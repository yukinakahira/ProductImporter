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
        public void MapGoldenBrandId(Product Product, bool isMappingEnabled, List<ApprovalPending> approvalPendings)
        {
            var brandMap = InMemoryBrandMapping.BrandMap;

            if (isMappingEnabled)
            {
                if (brandMap.TryGetValue(Product.BrandId, out var goldenBrandId))
                {
                    // マッピング成功：TempProductのGoldenBrandIdプロパティを更新します
                    Console.WriteLine($"[BrandMapping] TempProductのBrandId({Product.BrandId})をGoldenBrandId({goldenBrandId})にマッピングしました。");
                    Product.BrandId = goldenBrandId;
                }
                else
                {
                    var pendingId = InMemoryProductRepository.GetNextPendingId();
                    Logger.Info($"[BrandMapping] 新しい承認待ちアイテムが追加されました: PendingId={pendingId}");
                    // マッピング失敗：承認待ちリストに追加
                    Console.WriteLine($"[BrandMapping] TempProductのBrandId({Product.BrandId})はマッピングできませんでした。");
                    approvalPendings.Add(new ApprovalPending
                    {
                        PendingId = pendingId, // 自动生成唯一PendingId
                        PendingType = "BRAND",
                        OriginalId = Product.BrandId,
                        OriginalName = Product.BrandName,
                        UsageId = Product.ProductCode,
                        Remarks = "ブランドIDマッピングなし",
                        // CsvData = new Dictionary<string, string>
                        // {
                        //     { "TempProductCode", Product.ProductCode },
                        //     { "BrandId", Product.BrandId },
                        //     { "BrandName", Product.BrandName }
                        // }
                    });
                }
            }
            else
            {
                // マッピングが無効な場合：承認待ちリストに追加
                Console.WriteLine($"[BrandMapping] isMappingEnabled=falseのため、TempProductのBrandId({Product.BrandId})はマッピングしませんでした。");
                var pendingId = InMemoryProductRepository.GetNextPendingId();
                Logger.Info($"[BrandMapping] 新しい承認待ちアイテムが追加されました: PendingId={pendingId}");
                approvalPendings.Add(new ApprovalPending
                {
                    PendingId = pendingId,
                    OriginalId = Product.BrandId,
                    OriginalName = Product.BrandName,
                    UsageId = Product.ProductCode,
                    Remarks = "isMappingEnabled=falseのためマッピングせず",
                    // CsvData = new Dictionary<string, string>
                    // {
                    //     { "TempProductCode", Product.ProductCode },
                    //     { "BrandId", Product.BrandId },
                    //     { "BrandName", Product.BrandName }
                    // }
                });
            }
        }
    }

    // テスト実行用のクラス
    // public class BrandMappingTest
    // {
    //     public static void RunTest()
    //     {
    //         var approvalPendings = new List<ApprovalPending>();
    //         var brandMappingService = new BrandMappingService();

    //         // 1. マッピング成功テスト (BR001)
    //         var tempProduct1 = new TempProduct
    //         {
    //             ProductCode = "TEST001",
    //             BrandId = "BR001",
    //             BrandName = "LV"
    //         };
    //         brandMappingService.MapGoldenBrandId(tempProduct1, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct1.ProductCode}, GoldenBrandId={tempProduct1.GoldenBrandId}\n");


    //         // 2. マッピング失敗テスト (UNKNOWN)
    //         var tempProduct2 = new TempProduct
    //         {
    //             ProductCode = "TEST002",
    //             BrandId = "UNKNOWN",
    //             BrandName = "UNKNOWN"
    //         };
    //         brandMappingService.MapGoldenBrandId(tempProduct2, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct2.ProductCode}, GoldenBrandId={tempProduct2.GoldenBrandId}\n");


    //         // 3. マッピング成功テスト (BR002) - isMappingEnabledをtrueに変更
    //         var tempProduct3 = new TempProduct
    //         {
    //             ProductCode = "TEST003",
    //             BrandId = "BR007",
    //             BrandName = "ルイヴィトン"
    //         };
    //         brandMappingService.MapGoldenBrandId(tempProduct3, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct3.ProductCode}, GoldenBrandId={tempProduct3.GoldenBrandId}\n");

    //         // 3. マッピング成功テスト (BR002) - isMappingEnabledをtrueに変更
    //         var tempProduct4 = new TempProduct
    //         {
    //             ProductCode = "TEST004",
    //             BrandId = "BR003",
    //             BrandName = "シャネル"
    //         };
    //         brandMappingService.MapGoldenBrandId(tempProduct4, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct4.ProductCode}, GoldenBrandId={tempProduct4.GoldenBrandId}\n");

    //         // 3. マッピング成功テスト (BR002) - isMappingEnabledをtrueに変更
    //         var tempProduct5 = new TempProduct
    //         {
    //             ProductCode = "TEST005",
    //             BrandId = "BR001",
    //             BrandName = "CHANNEL"
    //         };
    //         brandMappingService.MapGoldenBrandId(tempProduct5, true, approvalPendings);
    //         Console.WriteLine($"--> 結果: ProductCode={tempProduct5.ProductCode}, GoldenBrandId={tempProduct5.GoldenBrandId}\n");
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
    //                 Console.WriteLine($"[Test] ApprovalPending: ID={pending.OriginalId}, Name={pending.OriginalName}, Remarks={pending.Remarks}");
    //             }
    //         }
    //     }
    // }

    // メインのエントリーポイント
    // class BrandMappingTestEntry
    // {
    //     static void Main()
    //     {
    //         BrandMappingTest.RunTest();
    //     }
    // }
}