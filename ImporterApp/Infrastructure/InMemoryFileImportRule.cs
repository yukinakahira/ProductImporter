//todo
//在这里写一个模拟数据,模拟实际上存在fileImportRule的设定值
//主要是为了从fileName和GpCompanyId获取fileImportRule的UsageId

using System.Collections.Generic;
using ImporterApp.Models;
namespace ImporterApp.Infrastructure
{
    // FileImportRuleDetailの疑似データ
    public class InMemoryFileImportRule
    {
        // ファイル名+会社IDからUsageIdを取得するための疑似マスタ
        public static readonly List<FileImportRule> FileImportRules = new List<FileImportRule>
        {
            new FileImportRule { FileName = "RKE伝票.csv", GpCompanyId = "RKE", UsageId = "RKE_PRODCT" },
            new FileImportRule { FileName = "KM商品.csv", GpCompanyId = "KM", UsageId = "KM_PRODCT" },
            // 必要に応じて追加
        };

        // fileNameとgpCompanyIdからUsageIdを取得
        public static string? GetUsageId(string fileName, string gpCompanyId)
        {
            var rule = FileImportRules.Find(r => r.FileName == fileName && r.GpCompanyId == gpCompanyId);
            return rule?.UsageId;
        }
    }
}