using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Pages;
using Moq;
using Xunit;

namespace UnitTests.Pages;

/// <summary>
/// Unit tests for the ErrorModel page
/// </summary>
public class ErrorModelTests
{
    private readonly ILogger<ErrorModel> _logger = Mock.Of<ILogger<ErrorModel>>();

    /// <summary>
    /// Creates a new ErrorModel instance with HttpContext for testing
    /// </summary>
    /// <param name="httpContext">Output parameter for the created HttpContext</param>
    /// <returns>A new ErrorModel instance with PageContext set</returns>
    private ErrorModel NewModel(out DefaultHttpContext httpContext)
    {
        httpContext = new DefaultHttpContext();
        var pageContext = new PageContext(
            new ActionContext(
                httpContext,
                new RouteData(),
                new PageActionDescriptor(),
                new ModelStateDictionary()))
        {
            HttpContext = httpContext
        };
        return new ErrorModel(_logger)
        {
            PageContext = pageContext
        };
    }

    /// <summary>
    /// Tests that the constructor creates a valid instance
    /// </summary>
    [Fact]
    public void Constructor_ValidLogger_CreatesInstance_WhenCalled_ShouldReturnNotNull()
    {
        // Arrange
        var data = _logger;
        
        // Act
        var result = new ErrorModel(data);
        
        // Assert
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests that OnGet uses trace identifier when no activity is present
    /// </summary>
    [Fact]
    public void OnGet_NoActivity_UsesTraceIdentifier_WhenCalled_ShouldSetRequestIdToTraceIdentifier()
    {
        // Arrange
        var data = NewModel(out var httpContext);
        httpContext.TraceIdentifier = "trace-id";

        // Act
        data.OnGet();

        // Assert
        Assert.Equal("trace-id", data.RequestId);
        Assert.True(data.ShowRequestId);
    }

    /// <summary>
    /// Tests that OnGet uses activity ID when an activity is present
    /// </summary>
    [Fact]
    public void OnGet_WithActivity_UsesActivityId_WhenCalled_ShouldSetRequestIdToActivityId()
    {
        // Arrange
        var activity = new Activity("test").Start();
        var data = NewModel(out var httpContext);
        httpContext.TraceIdentifier = "ignored";

        // Act
        data.OnGet();
        var result = data.RequestId;

        // Assert
        Assert.Equal(activity.Id, result);
        Assert.True(data.ShowRequestId);

        activity.Stop();
    }
}
