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
                "ブランドIDマッピング失敗",
                new Dictionary<string, string> {
                    { "ProductCode", product.ProductCode },
                    { "ProductName", product.ProductName },
                    { "BrandId", product.BrandId },
                    { "BrandName", product.BrandName }
                }
            );
            return approvalPendings;
        }
        //カテゴリマッピングする
        public List<ApprovalPending> ExeuteCategoryMapping(Product product)
        {
            var categoryMappingService = new CategoryMappingService();
            bool isMapped = categoryMappingService.CategoryMapping(product.CategoryId);
            var approvalPendings = new List<ApprovalPending>();
            var mappingLogic = new YourProject.Services.MappingLogic();
            // ブランドマッピング
            mappingLogic.MapCommon(
                isMapped,
                product,
                approvalPendings,
                "CATEGORY",
                product.CategoryId,
                product.CategoryName,
                $"[CategoryMapping] TempProductのCategoryId({product.CategoryId})はマッピング成功。",
                $"[CategoryMapping] TempProductのCategoryId({product.CategoryId})はマッピング失敗。",
                "カテゴリIDマッピング失敗",
                new Dictionary<string, string> {
                    { "ProductCode", product.ProductCode },
                    { "ProductName", product.ProductName },
                    { "BrandId", product.BrandId },
                    { "BrandName", product.BrandName }
                }
            );
            return approvalPendings;
        }
    }
}