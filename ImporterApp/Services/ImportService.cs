using ImporterApp.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using ImporterApp.Infrastructure;
using ImporterApp.Rules;

namespace ImporterApp.Services
{
    
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly RuleEngine _ruleEngine;

        public ImportService(RuleEngine ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        // staging.csvの1行を処理し、Productオブジェクトを返す
        // userScenarioIdは将来の拡張用、現在は使用しない
        public Product ProcessRow(Dictionary<string, string> rowData, string usageId)
        {
            try
            {
                Product product = new();
                Logger.Info($"[IMPORT] 開始: usageId={usageId}, rowData={string.Join(", ", rowData.Select(kv => $"{kv.Key}={kv.Value}"))}");
                product = RuleExecutor.ExecuteRules(_ruleEngine.Rules, rowData, usageId);
                //BrandMappingServiceを使用して、ProductのBrandIdをゴールデンブランドIDにマッピング
                var brandMappingService = new BrandMappingService();
                var approvalPendings = new List<ApprovalPending>();
                brandMappingService.MapGoldenBrandId(product, true, approvalPendings);
                // ApprovalPendingを全局リストにも追加
                foreach (var pending in approvalPendings)
                {
                    InMemoryProductRepository.PendingBrands.Add(pending);
                }
                Logger.Info($"[IMPORT] ProductCode={product.ProductCode}, BrandId={product.BrandId}, ProductName={product.ProductName}, Category={product.CategoryName},State={product.State}, Attributes={string.Join(", ", product.Attributes.Select(a => $"{a.AttributeId}={a.Value}"))}");
                return product;
            }
            catch (Exception ex)
            {
                Logger.Info($"[FATAL] ImportService.ProcessRow 例外: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
        
    }
}
