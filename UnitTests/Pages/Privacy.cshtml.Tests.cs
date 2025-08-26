using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Unit tests for the PrivacyModel page
/// </summary>
public class PrivacyModelTests
{
    private readonly ILogger<PrivacyModel> _logger = Mock.Of<ILogger<PrivacyModel>>();

    /// <summary>
    /// Creates a new PrivacyModel instance for testing
    /// </summary>
    /// <returns>A new PrivacyModel instance</returns>
    private PrivacyModel NewModel() => new(_logger);

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
        
        // Act & Assert - should execute without throwing
        data.OnGet();
    }
}