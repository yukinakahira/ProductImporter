using ImporterApp.Models;
using ImporterApp.Infrastructure;
using ImporterApp.Rules;
using ImporterApp.Services.Shared;
using ImporterApp.Services.Execution;

namespace ImporterApp.Services
{
    
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly ImporterApp.Rules.RuleEngine _ruleEngine;

        public ImportService(ImporterApp.Rules.RuleEngine ruleEngine)
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
                // マッピングロジックの実行
                var mappingExecutor = new MappingExecutor();
                //ブランドマッピング
                var brandPendings = mappingExecutor.ExecuteBrandMapping(product);
                InMemoryProductRepository.PendingBrands.AddRange(brandPendings);
                // カテゴリマッピング
                var categoryPendings = mappingExecutor.ExeuteCategoryMapping(product);
                InMemoryProductRepository.PendingBrands.AddRange(categoryPendings);
                // アイテムリストマッピング
                // カラーを例に
                var itemListPendings = mappingExecutor.ExecuteItemListMapping(product);
                InMemoryProductRepository.PendingBrands.AddRange(itemListPendings);
                
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
