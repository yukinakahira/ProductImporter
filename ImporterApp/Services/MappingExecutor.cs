using ImporterApp.Infrastructure;
using ImporterApp.Models;
using System; // For Logger (assuming it's in System or a base namespace)
using System.Collections.Generic;

namespace ImporterApp.Services
{
    public class MappingExecutor
    {
        //ブランドマッピングする
        public List<ApprovalPending> ExecuteBrandMapping(Product product)
        {
            var brandMappingService = new BrandMappingService();
            bool isMapped = brandMappingService.BrandMapping(product.BrandId);
            var approvalPendings = new List<ApprovalPending>();
            var mappingLogic = new YourProject.Services.MappingLogic();
            // ブランドマッピング
            mappingLogic.MapCommon(
                isMapped,
                product,
                approvalPendings,
                "BRAND",
                product.BrandId,
                product.BrandName,
                $"[BrandMapping] TempProductのBrandId({product.BrandId})はマッピング成功。",
                $"[BrandMapping] TempProductのBrandId({product.BrandId})はマッピングできませんでした。",
                "ブランドIDマッピングなし",
                new Dictionary<string, string> {
                    { "ProductCode", product.ProductCode },
                    { "BrandId", product.BrandId },
                    { "BrandName", product.BrandName }
                }
            );
            // TODO: カテゴリ、物品等の他のMapCommonも同様に呼び出し可能
            // approvalPendings 可在主流程统一收集・输出
            return approvalPendings;
        }
        //カテゴリマッピングする
        public List<ApprovalPending> ExeuteCategoryMapping(Product product)
        {
            var categoryMappingService = new CategoryMappingService();
            bool isMapped = categoryMappingService.CategoryMapping(product.BrandId);
            var approvalPendings = new List<ApprovalPending>();
            var mappingLogic = new YourProject.Services.MappingLogic();
            // ブランドマッピング
            mappingLogic.MapCommon(
                isMapped,
                product,
                approvalPendings,
                "BRAND",
                product.BrandId,
                product.BrandName,
                $"[BrandMapping] TempProductのBrandId({product.BrandId})はマッピング成功。",
                $"[BrandMapping] TempProductのBrandId({product.BrandId})はマッピングできませんでした。",
                "ブランドIDマッピングなし",
                new Dictionary<string, string> {
                    { "ProductCode", product.ProductCode },
                    { "BrandId", product.BrandId },
                    { "BrandName", product.BrandName }
                }
            );
            // TODO: カテゴリ、物品等の他のMapCommonも同様に呼び出し可能
            // approvalPendings 可在主流程统一收集・输出
            return approvalPendings;
        }
    }
}