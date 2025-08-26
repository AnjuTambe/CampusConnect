using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Test class for AboutUsModel page functionality
/// Tests page initialization and OnGet method behavior
/// </summary>
public class AboutUsModelTests
{
    /// <summary>
    /// Mock logger for testing
    /// </summary>
    private readonly ILogger<AboutUsModel> _logger = Mock.Of<ILogger<AboutUsModel>>();

    /// <summary>
    /// Helper method to create AboutUsModel instance for testing
    /// </summary>
    /// <returns>Configured AboutUsModel instance</returns>
    private AboutUsModel NewModel() => new(_logger);

    /// <summary>
    /// Tests that AboutUsModel constructor creates instance successfully
    /// </summary>
    [Fact]
    public void Constructor_Valid_Test_Creation_Should_Return_Instance()
    {
        // Arrange
        var data = _logger;

        // Act
        var result = NewModel();
        
        // Assert
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests that OnGet method executes without throwing exceptions
    /// </summary>
    [Fact]
    public void OnGet_Valid_Test_Execution_Should_Return_NoThrow()
    {
        // Arrange
        var data = NewModel();
        
        // Act
        var result = Record.Exception(() => data.OnGet());
        
        // Assert
        Assert.Null(result);
    }
}