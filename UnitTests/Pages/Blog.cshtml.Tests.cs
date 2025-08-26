using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Unit tests for the BlogModel page
/// </summary>
public class BlogModelTests
{
    private readonly ILogger<BlogModel> _logger = Mock.Of<ILogger<BlogModel>>();

    /// <summary>
    /// Creates a new BlogModel instance for testing
    /// </summary>
    /// <returns>A new BlogModel instance</returns>
    private BlogModel NewModel() => new(_logger);

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
        
        // Act & Assert - should not throw
        data.OnGet();
    }
}