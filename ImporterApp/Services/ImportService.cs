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
                    var itemId = rule.ItemId;
                    var resultValue = rule.ResultValue;
                    string value = null;

                    // Fixed: 入力値をそのまま
                    if (outType == "Fixed" || outType == "そのまま登録")
                    {
                        rowData.TryGetValue(col, out value);
                        value ??= string.Empty;
                        if (table == "PRODUCT_MST")
                        {
                            // 主属性
                            if (itemId == "PRODUCT_CODE") product.ProductCode = value;
                            else if (itemId == "BRAND_ID") product.BrandId = value;
                            else if (itemId == "PRODUCT_NAME") product.ProductName = value;
                            else if (col == "category" || itemId == "CATEGORY" || col == "COL_7") product.category = value;
                            // 追加のPRODUCT_MST属性があればここに
                        }
                        else if (table == "PRODUCT_EAV")
                        {
                            var mappedId = _ruleEngine.MapAttributeId(itemId, product.category);
                            product.Attributes.Add(new ProductAttribute
                            {
                                AttributeId = mappedId,
                                Value = value
                            });
                        }
                        Logger.Info($"[RULE:{rule.RuleId}] FIXED: {itemId} ← {value} (from {col})");
                    }
                    // Transform: 結果値をセット
                    else if (outType == "Transform" || outType == "変換して登録")
                    {
                        value = resultValue;
                        if (table == "PRODUCT_MST")
                        {
                            if (itemId == "PRODUCT_CODE") product.ProductCode = value;
                            else if (itemId == "BRAND_ID") product.BrandId = value;
                            else if (itemId == "PRODUCT_NAME") product.ProductName = value;
                            else if (col == "category" || itemId == "CATEGORY") product.category = value;
                        }
                        else if (table == "PRODUCT_EAV")
                        {
                            var mappedId = _ruleEngine.MapAttributeId(itemId, product.category);
                            product.Attributes.Add(new ProductAttribute
                            {
                                AttributeId = mappedId,
                                Value = value
                            });
                        }
                        Logger.Info($"[RULE:{rule.RuleId}] TRANSFORM: {itemId} ← {value} (from {col})");
                    }
                }
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
