

namespace ImporterApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using ImporterApp.Infrastructure;
    using ImporterApp.Models;

    // ブランドマッピングサービス
    public class BrandMapping
    {
        // // 連携元ブランドID → ゴールデンブランドIDのマッピング
        // public static Dictionary<string, string> GetBrandMap()
        // {
        //     return InMemoryBrandMapping.BrandMap;
        // }

        // // 連携元ブランドID → ゴールデンブランド名のマッピング
        // public static Dictionary<string, string> GetBeforeBrandMap()
        // {
        //     return InMemoryBrandMapping.BeforeBrandMap;
        // }

        //TODO
        // ブランドIDをゴールデンブランドIDにマッピングするメソッド
        // Product対象から連携元ブランドID（Product.BrandID）を取得する
        // Product.BrandIDを使って、GetBrandMapの中にGoldenブランドIDとマッピングするメソッドを書いてください
        //メソッドの名前は BrandMappingClassにする、isMappedのboolean型の引数を受け取る
        //isMappedがtrueの場合は、GetBrandMapの中にGoldenブランドIDとマッピングする
        //終わったら、Loggerを書いてください
        //ProductのBrandIdをGoldenブランドIDに変換する
        //isMappedがfalseの場合は、GetBrandMapの中にGoldenブランドIDとマッピングしない
        //Product対象からProductcodeを取得し、Productcodeで対象のデータをModels/ApprovalPending.csに登録する
        //終わったら、Loggerを書いてください

        // ...existing code...
        /// <summary>
        /// ブランドIDをゴールデンブランドIDにマッピングするメソッド
        /// </summary>
        /// <param name="product">対象Product</param>
        /// <param name="isMapped">マッピングするかどうか</param>
        /// <param name="approvalPendings">マッピングできなかった場合に追加するリスト</param>
        public void BrandMappingClass(Product product, bool isMapped, List<ApprovalPending> approvalPendings)
        {
            var brandMap = InMemoryBrandMapping.BrandMap;
            // Logger is static, so use its static methods directly

            if (isMapped)
            {
                if (brandMap.TryGetValue(product.BrandId, out var goldenBrandId))
                {
                    Logger.Info($"[BrandMapping] ProductCode: {product.ProductCode} のBrandId({product.BrandId})をGoldenBrandId({goldenBrandId})にマッピングしました。");
                    product.BrandId = goldenBrandId;
                }
                else
                {
                    Logger.Warn($"[BrandMapping] ProductCode: {product.ProductCode} のBrandId({product.BrandId})はマッピングできませんでした。");
                    approvalPendings.Add(new ApprovalPending
                    {
                        PendingType = "BRAND",
                        OriginalId = product.BrandId,
                        OriginalName = product.BrandName,
                        UsageId = product.ProductCode,
                        Remarks = "ブランドIDマッピングなし",
                        CsvData = new Dictionary<string, string>
                        {
                            { "ProductCode", product.ProductCode },
                            { "BrandId", product.BrandId },
                            { "BrandName", product.BrandName }
                        }
                    });
                }
            }
            else
            {
                Logger.Info($"[BrandMapping] isMapped=falseのため、ProductCode: {product.ProductCode} のBrandId({product.BrandId})はマッピングしませんでした。");
                approvalPendings.Add(new ApprovalPending
                {
                    PendingType = "BRAND",
                    OriginalId = product.BrandId,
                    OriginalName = product.BrandName,
                    UsageId = product.ProductCode,
                    Remarks = "isMapped=falseのためマッピングせず",
                    CsvData = new Dictionary<string, string>
                    {
                        { "ProductCode", product.ProductCode },
                        { "BrandId", product.BrandId },
                        { "BrandName", product.BrandName }
                    }
                });
            }
        }
    }
}