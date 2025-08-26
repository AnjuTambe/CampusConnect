using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Unit tests for the HowItWorksModel page
/// </summary>
public class HowItWorksModelTests
{
    private readonly ILogger<HowItWorksModel> _logger = Mock.Of<ILogger<HowItWorksModel>>();

    /// <summary>
    /// Creates a new HowItWorksModel instance for testing
    /// </summary>
    /// <returns>A new HowItWorksModel instance</returns>
    private HowItWorksModel NewModel() => new(_logger);

    /// <summary>
    /// Tests that the constructor creates a valid instance
    /// </summary>
    [Fact]
    public void Constructor_ValidInput_CreatesInstance_WhenCalled_ShouldReturnNotNull()
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
    public void OnGet_NoParameters_ExecutesSuccessfully_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var data = NewModel();
        
        // Act & Assert - OnGet should execute without throwing
        data.OnGet();
    }
}
