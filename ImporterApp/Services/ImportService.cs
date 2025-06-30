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
        public Product ProcessRow(Dictionary<string, string> rowData, string userScenarioId)
        {
<<<<<<< HEAD
            Logger.Info($"[INFO] Start processing row: {string.Join(", ", rowData)}");

            // ① ルール取得
            var ruleDetails = _ruleRepo.GetRules(userScenarioId);
            Logger.Info($"[INFO] Rules loaded: {ruleDetails.Count}");

            // 商品種別を確定する
            rowData.TryGetValue("COL_7", out var productType); // COL_7 = 商品種別
            Logger.Info($"[INFO] 商品種別: {productType}");

            // 商品種別にマッチするルールだけを抽出
            var applicableRules = ruleDetails
            .Where(r => string.IsNullOrEmpty(r.ProductType) || r.ProductType == productType)
            .ToList();

            // ② Product初期化
            var product = new Product();

            // ③ 各ルールに従ってマッピング適用
            foreach (var rule in applicableRules)
            {
                if (!rowData.TryGetValue(rule.ColumnName, out var value)) continue;

                Logger.Info($"[INFO] Applying ColumnName: {rule.ColumnName} → AttributeId: {rule.AttributeId} = {value}");

                if (rule.SaveTable == "PRODUCT_MST")
                {
                    switch (rule.AttributeId)
                    {
                        case "PRODUCT_CODE":
                            product.ProductCode = value;
                            break;
                        case "BRAND_ID":
                            product.BrandId = value;
                            break;
                        case "PRODUCT_NAME":
                            product.ProductName = value;
                            break;
                        case "PRODUCT_TYPE":
                            product.ProductType = value;
                            break;
                    }

                    Logger.Info($"[INFO] Set {rule.AttributeId} = {value}");
                }
                else if (rule.SaveTable == "PRODUCT_EAV")
                {
                    product.Attributes.Add(new ProductAttribute
                    {
                        AttributeId = rule.AttributeId,
                        Value = value
                    });


                    Logger.Info($"[INFO] Set EAV Attribute: {rule.AttributeId} = {value}");
                }
=======
            try
            {
                Product product = new();
                Logger.Info($"[IMPORT] 開始: userScenarioId={userScenarioId}, rowData={string.Join(", ", rowData.Select(kv => $"{kv.Key}={kv.Value}"))}");
                product = RuleExecutor.ExecuteRules(_ruleEngine.Rules, rowData);
                Logger.Info($"[IMPORT] ProductCode={product.ProductCode}, BrandId={product.BrandId}, ProductName={product.ProductName}, Category={product.Category},State={product.State}, Attributes={string.Join(", ", product.Attributes.Select(a => $"{a.AttributeId}={a.Value}"))}");
                return product;
            }
            catch (Exception ex)
            {
                Logger.Info($"[FATAL] ImportService.ProcessRow 例外: {ex.Message}\n{ex.StackTrace}");
                throw;
>>>>>>> trunk_20250623
            }
        }
    }
}
