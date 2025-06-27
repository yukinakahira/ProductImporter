using ImporterApp.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using ImporterApp.Infrastructure;

namespace ImporterApp.Services
{
    // ステージングデータとマッピングルールのマッチング
    public class ImportService
    {
        private readonly IRuleEngine _ruleEngine;

        public ImportService(IRuleEngine ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        // staging.csvの1行を処理し、Productオブジェクトを返す
        // userScenarioIdは将来の拡張用、現在は使用しない
        public Product ProcessRow(Dictionary<string, string> rowData, string userScenarioId)
        {
            try
            {
                var product = new Product();

                foreach (var rule in _ruleEngine.Rules)
                {
                    // 取得対象カラム
                    var col = rule.TargetColumn;
                    var outType = rule.OutType;
                    var table = rule.TargetTable;
                    var column = rule.TargetColumn;
                    var itemId = rule.ItemId;
                    var resultValue = rule.ResultValue;
                    string value = string.Empty;

                    // 条件チェック
                    if (outType == "Fixed" || outType == "そのまま登録")
                    {
                        // 固定値の場合、直接使用 ResultValue
                        if (!string.IsNullOrEmpty(col) && rowData.TryGetValue(col, out var tmpVal) && tmpVal != null)
                            value = tmpVal;
                        else if (!string.IsNullOrEmpty(itemId) && rowData.TryGetValue(itemId, out var tmpVal2) && tmpVal2 != null)
                            value = tmpVal2;
                        else
                            value = string.Empty;
                        Logger.Info($"[RULE:{rule.RuleId}] FIXED: {itemId} ← {value} (from {col}) [保存対象テーブル:{table}, 保存対象カラム:{column}]");
                    }
                    else if (outType == "Transform" || outType == "変換して登録")
                    {
                        value = resultValue ?? string.Empty;
                        Logger.Info($"[RULE:{rule.RuleId}] TRANSFORM: {itemId} ← {value} (from {col}) [保存対象テーブル:{table}, 保存対象カラム:{column}]");
                    }
                    else
                    {
                        continue; // 未知のタイプはスキップ
                    }

                    // 主属性赋值已移至 MapMainProperties
                    if (table == "PRODUCT_EAV")
                    {
                        var mappedId = _ruleEngine.MapAttributeId(itemId ?? string.Empty, product.Category ?? string.Empty);

                        if (!product.Attributes.Any(a => a.AttributeId == mappedId))
                        {
                            product.Attributes.Add(new ProductAttribute
                            {
                                AttributeId = mappedId,
                                Value = value ?? string.Empty
                            });
                        }
                    }
                    // 他のテーブルがあればここに
                }

                // 主属性赋值应在所有规则处理后
                Logger.Info($"[DEBUG] 2rowData: {string.Join(", ", rowData.Select(kv => $"[{kv.Key}, {kv.Value}]") )}");
                _ruleEngine.MapMainProperties(product, rowData);
                Logger.Info($"[DEBUG]Properties of product: {product.ProductCode}");

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
