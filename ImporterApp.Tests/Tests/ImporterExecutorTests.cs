using System.IO;
using System.Linq;
using ImporterApp;
using ImporterApp.Models;
using ImporterApp.Infrastructure; 
using Xunit;

public class ImporterExecutorTests
{
    [Fact]
    public void Execute_ShouldImportCsvDataCorrectly()
    {
        // Arrange
        InMemoryProductRepository.Products.Clear(); // テスト開始前に初期化
        var csvPath = "test_data.csv";
        var scenarioId = "ユースジID1";

        var executor = new ImporterExecutor();

        // Act
        executor.Execute(csvPath, scenarioId);

        // Assert
        Assert.Equal(3, InMemoryProductRepository.Products.Count);

        var product = InMemoryProductRepository.Products.First(p => p.ProductCode == "P001");
        Assert.Equal("BR001", product.BrandId);
        Assert.Equal("ショルダーバッグ", product.ProductName);
        Assert.Equal(4, product.Attributes.Count); // EAV項目数

        var size1 = product.Attributes.FirstOrDefault(a => a.AttributeId == "SIZE_1");
        Assert.Equal("13", size1?.Value);
    }
}
