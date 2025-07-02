using System.Collections.Generic;
using ImporterApp.Models;

namespace ImporterApp.Infrastructure
{
    // 現在登録中のデータの疑似データ表現
    public static class InMemoryProductRepository
    {
        public static List<Product> Products = new List<Product>
        {
            new Product
            {
                ProductCode = "P002",
                BrandId = "BR000",
                CategoryName = "バッグ",
                ProductName = "トートバッグ旧"
            },
            new Product
            {
                ProductCode = "P003",
                BrandId = "BR003",
                CategoryName = "バッグ",
                ProductName = "リュック"
            }
        };

        public static List<ProductHistory> Histories { get; set; } = new();
        public static List<ApprovalPending> PendingBrands { get; set; } = new();
        // PD用の自增カウンタ
        private static int _pendingIdCounter = 1;
        public static string GetNextPendingId()
        {
            return $"PD{_pendingIdCounter++.ToString("D6")}";
        }
        public static void Clear()
        {
            Products = new();
            Histories = new();
            PendingBrands = new();
            _pendingIdCounter = 1;
        }
    }
}
