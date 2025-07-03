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
        // アイテムリストマッピングする
        public List<ApprovalPending> ExecuteItemListMapping(Product product)
        {
            var itemListMappingService = new ItemListMappingService();
            var approvalPendings = new List<ApprovalPending>();
            var mappingLogic = new YourProject.Services.MappingLogic();
            // Product.Attributes から全ての属性（如颜色、尺寸等）をマッピング
            if (product.Attributes != null)
            {
                foreach (var attr in product.Attributes)
                {
                    string itemId = attr.AttributeId; // 例：COLOR_ID
                    string itemListId = attr.Value; // 例：COLOR001
                    bool isMapped = itemListMappingService.MapItemList(itemId,itemListId);
                    mappingLogic.MapCommon(
                        isMapped,
                        product,
                        approvalPendings,
                        itemId.ToUpper(),
                        itemId,
                        itemListId,
                        $"[ItemListMapping] {itemId}({itemListId})はマッピング成功。",
                        $"[ItemListMapping] {itemId}({itemListId})はマッピング失敗。",
                        $"項目リスト{itemId}マッピング失敗",
                        new Dictionary<string, string> {
                            { "ProductCode", product.ProductCode },
                            { "ProductName", product.ProductName },
                            { $"{itemId}Id", itemId }
                        }
                    );
                }
            }
            return approvalPendings;
        }
    }
}