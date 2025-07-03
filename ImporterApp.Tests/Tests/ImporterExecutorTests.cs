using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using ImporterApp.Models;
using ImporterApp.Services;
// Make sure to use the namespace where your mock data is located
using ImporterApp.Tests; 

public class ImporterExecutorTests
{
    private readonly ITestOutputHelper _output;

    public ImporterExecutorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    // Test Case 1: A brand ID that exists IS successfully mapped.
    [Fact]
    public void MapGoldenBrandId_ShouldSucceed_WhenBrandIdExists()
    {
        // Arrange
        var brandMappingService = new BrandMappingService();
        var approvalPendings = new List<ApprovalPending>();
        // Using "BR001" which exists in our mock InMemoryBrandMapping
        Product product = new() { BrandId = "BR001", BrandName = "LV" };

        // Act
        // isMappingEnabled is true, so it should find the mapping
        brandMappingService.MapGoldenBrandId(product, approvalPendings);

        // Log the result to the test output
        _output.WriteLine($"Testing successful mapping for BrandId: {product.BrandId}");
        _output.WriteLine($"Pending list count: {approvalPendings.Count}");

        // Assert
        // The BrandId should be updated to the "golden" one from our mock data.
        Assert.Equal("GoldenBR001", product.BrandId);
        // No items should be added to the pending list on success.
        Assert.Empty(approvalPendings); 
    }

    // Test Case 2: A product IS ADDED to the pending list if mapping is disabled.
    [Fact]
    public void MapGoldenBrandId_ShouldAddToPending_WhenMappingIsDisabled()
    {
        // Arrange
        var brandMappingService = new BrandMappingService();
        var approvalPendings = new List<ApprovalPending>();
        Product product = new() { BrandId = "BR006", BrandName = "Unknown Brand" };
        // var product = new Product { BrandId = "GoldenBR001", BrandName = "LV" };

        // Act
        // isMappingEnabled is false, so it should be added to the pending list.
        brandMappingService.MapGoldenBrandId(product,  approvalPendings);

        // Log the result to the test output
        _output.WriteLine($"Testing disabled mapping for BrandId: {product.BrandId}");
        _output.WriteLine($"Pending list count: {approvalPendings.Count}");

        // Assert
        // The list should now contain exactly one item.
        Assert.Single(approvalPendings);
        // The original ID in the pending item should be "BR007".
        Assert.Equal("BR007", approvalPendings[0].OriginalId);
        // Assert.Equal("GoldenBR001", product.BrandId);
    }
}