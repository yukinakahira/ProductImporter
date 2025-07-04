using System;
using System.Collections.Generic;
using ImporterApp.Models;
using ImporterApp.Services.Shared;

namespace ImporterApp.Services.Mapping
{
    /// <summary>
    /// Prodcut属性マッピング
    /// </summary>
    public static class ProductPropertyMapping
    {
        public static void ApplyRuleToProduct(RuleGroup rule, Dictionary<string, string> rowData, Product product)
        {
            string value = string.Empty;
            if (rule.OutType == "そのまま登録")
            {
                var key = rule.ResultValue;
                rowData.TryGetValue(key, out value);
                Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}、{rule.TargetTable}の{rule.TargetColumn}に値を設定：{value}");
            }
            else if (rule.OutType == "変換して登録")
            {
                value = rule.ResultValue ?? string.Empty;
            }
            // Productモデルに書き込み
            if (rule.TargetTable == "PRODUCT_MST")
            {
                switch (rule.TargetColumn)
                {
                    case "PRODUCT_CODE": product.ProductCode = value ?? string.Empty; break;
                    case "PRODUCT_NAME": product.ProductName = value ?? string.Empty; break;
                    case "BRAND_ID": product.BrandId = value ?? string.Empty; break;
                    case "BRAND_NAME": product.BrandName = value ?? string.Empty; break;
                    case "CATEGORY_ID": product.CategoryId = value ?? string.Empty; break;
                    case "CATEGORY_NAME": product.CategoryName = value ?? string.Empty; break;
                    case "PRODUCT_STATUS_CODE": product.ProductStatusCode = value ?? string.Empty; break;
                    case "STOCK_AVAILABILITY": product.StockAvailability = value ?? string.Empty; break;
                    case "SALE_AVAILABILITY": product.SaleAvailability = value ?? string.Empty; break;
                    case "STORE_ID": product.StoreId = value ?? string.Empty; break;
                    case "STORE_NAME": product.StoreName = value ?? string.Empty; break;
                    case "NEW_ITEM_TYPE": product.NewItemType = value ?? string.Empty; break;
                    case "QUANTITY":
                        if (int.TryParse(value, out int quantity))
                            product.Quantity = quantity;
                        else
                            product.Quantity = 0;
                        break;
                    case "SUPPLIER_TYPE": product.SupplierType = value ?? string.Empty; break;
                    case "SALES_DESTINATION_TYPE": product.SalesDestinationType = value ?? string.Empty; break;
                    case "SALES_DESTINATION_NAME": product.SalesDestinationName = value ?? string.Empty; break;
                    case "ASSESSMENT_PRICE":
                        if (decimal.TryParse(value, out decimal assessmentPrice))
                            product.AssessmentPrice = assessmentPrice;
                        else
                            product.AssessmentPrice = 0m;
                        break;
                    case "PURCHASE_PRICE": product.PurchasePrice = value ?? string.Empty; break;
                    case "DISPLAY_PRICE": product.DisplayPrice = value ?? string.Empty; break;
                    case "SALE_PRICE": product.SalesPrice = value ?? string.Empty; break;
                    case "CONSIGNMENT_GPCOMPANY_ID": product.ConsignmentGpCompanyId = value ?? string.Empty; break;
                    case "CONSIGNED_PRODUCT_CODE": product.ConsignedProductCode = value ?? string.Empty; break;
                    case "STAFF_NAME": product.StaffName = value ?? string.Empty; break;
                    case "UPDATE_TYPE": product.UpdateType = value ?? string.Empty; break;
                    case "UPDATE_DATE": product.UpdateDate = value ?? string.Empty; break;
                    case "UPDATE_TIME": product.UpdateTime = value ?? string.Empty; break;
                    default: break;
                }
            }
            else if (rule.TargetTable == "PRODUCT_EAV")
            {
                if (!string.IsNullOrEmpty(rule.ItemId) && !product.Attributes.Exists(a => a.AttributeId == rule.ItemId))
                {
                    product.Attributes.Add(new ProductAttribute { AttributeId = rule.ItemId, Value = value ?? string.Empty });

                    Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}、{rule.TargetTable}の{rule.TargetColumn}に値を設定：AttributeId={rule.ItemId}, Value={value}");
                }
            }
            //Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}、{rule.TargetTable}の{rule.TargetColumn}に値を設定：{value}");
            // return product;
        }
    }
}