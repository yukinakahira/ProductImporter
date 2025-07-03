//这个文件里需要做的事情是
//使用三个方法，分别名为BrandMapping、CategoryMapping和ItemListMapping，
//它们都返回True或False，表示是否成功映射了品牌、类别或物品列表。
//现在这三个方法并没有被实装，所以用TOdo来标记
//Mapping的总逻辑是如果这三个方法返回true则登录相关数据到Product对象中，
//否则将相关数据登录到ApprovalPending列表中等待审核。
//在到Services/ImporterExecutor.cs输出Map成功结果一览和待审核结果一览
using ImporterApp.Models;
using ImporterApp.Infrastructure;
using System.Collections.Generic;
using ImporterApp.Services;

namespace YourProject.Services
{
    public class MappingLogic
    {
        /// <summary>
        /// 汎用マッピング処理ロジック。ブランド・カテゴリ・アイテム等、複数種別の共通マッピング呼び出しに対応。
        /// </summary>
        /// <param name="isMapped">マッピング結果（成功/失敗）</param>
        /// <param name="product">商品オブジェクト</param>
        /// <param name="approvalPendings">承認待ちリスト</param>
        /// <param name="pendingType">承認待ち種別（例：BRAND、CATEGORY等）</param>
        /// <param name="originalId">元ID</param>
        /// <param name="originalName">元名称</param>
        /// <param name="successLog">マッピング成功時のログ出力</param>
        /// <param name="failLog">マッピング失敗時のログ出力</param>
        /// <param name="remarks">失敗時の備考</param>
        /// <param name="csvData">失敗時のCSVデータ</param>
        public void MapCommon(
            bool isMapped,
            Product product,
            List<ApprovalPending> approvalPendings,
            string pendingType,
            string originalId,
            string originalName,
            string successLog,
            string failLog,
            string remarks,
            Dictionary<string, string> csvData)
        {
            if (isMapped)
            {
                Console.WriteLine(successLog);
            }
            else
            {
                var pendingId = InMemoryProductRepository.GetNextPendingId();
                Logger.Info($"[{pendingType}Mapping] 新しい承認待ちアイテムが追加されました: PendingId={pendingId}");
                Console.WriteLine(failLog);
                approvalPendings.Add(new ApprovalPending
                {
                    PendingId = pendingId,
                    PendingType = pendingType,
                    OriginalId = originalId,
                    OriginalName = originalName,
                    UsageId = product.ProductCode,
                    Remarks = remarks,
                    CsvData = csvData
                });
            }
        }
    }
}
