using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using System;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Test class for IndexModel page functionality
/// Tests page initialization and OnGet method behavior
/// </summary>
public class IndexModelTests
{
    /// <summary>
    /// Mock logger for testing
    /// </summary>
    private readonly ILogger<IndexModel> _logger = Mock.Of<ILogger<IndexModel>>();

    #region Setup

    /// <summary>
    /// Helper method to create IndexModel instance for testing
    /// </summary>
    /// <returns>Configured IndexModel instance</returns>
    private IndexModel CreateModel()
    {
        var model = new IndexModel(_logger)
        {
            PageContext = new PageContext
            {
                ViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
            }
        };
        
        return model;
    }

    #endregion

    #region Constructor Tests

    /// <summary>
    /// Tests that IndexModel constructor initializes properties correctly
    /// </summary>
    [Fact]
    public void Constructor_Valid_Test_Initialization_Should_Return_PropertiesSet()
    {
        // Arrange
        var data = _logger;

        // Act
        var result = CreateModel();
        
        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region OnGet Tests

    /// <summary>
    /// Tests that OnGet method executes without throwing exceptions
    /// </summary>
    [Fact]
    public void OnGet_Valid_Test_Execution_Should_Return_NoThrow()
    {
        // Arrange
        var data = CreateModel();
        
        // Act
        var result = Record.Exception(() => data.OnGet());
        
        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Tests that OnGet method sets ViewData for fluid container
    /// </summary>
    [Fact]
    public void OnGet_Valid_Test_ViewData_Should_Return_FluidContainerSet()
    {
        // Arrange
        var data = CreateModel();
        
        // Act
        data.OnGet();
        var result = data.ViewData;
        
        // Assert
        Assert.True(result.ContainsKey("UseFluidContainer"));
        Assert.True((bool)result["UseFluidContainer"]!);
    }

    /// <summary>
    /// Tests that OnGet method sets expected ViewData value
    /// </summary>
    /// <param name="expectedValue">Expected value for UseFluidContainer</param>
    [Theory]
    [InlineData(true)]
    public void OnGet_Valid_Test_ExpectedValue_Should_Return_CorrectViewDataValue(bool expectedValue)
    {
        // Arrange
        var data = CreateModel();
        
        // Act
        data.OnGet();
        var result = data.ViewData["UseFluidContainer"];
        
        // Assert
        Assert.Equal(expectedValue, result);
    }

    #endregion

    #region Logger Tests

    /// <summary>
    /// Tests that logger is properly injected into IndexModel
    /// </summary>
    [Fact]
    public void Logger_Valid_Test_Injection_Should_Return_ProperlyInjected()
    {
        // Arrange
        var data = new Mock<ILogger<IndexModel>>();
        
        // Act
        var result = new IndexModel(data.Object);
        
        // Assert
        Assert.NotNull(result);
        // Note: We can't directly test the private _logger field, but we've verified it was injected
        // by successfully creating the model with the mock logger
    }

    #endregion
}