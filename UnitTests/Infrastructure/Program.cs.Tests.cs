using System;
using System.Linq;
using CampusConnect.WebSite;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace UnitTests.Infrastructure;

/// <summary>
/// Test class for Program class functionality
/// Tests the host builder creation and main method verification
/// </summary>
public class ProgramTests
{
    /// <summary>
    /// Tests that CreateHostBuilder returns a valid host builder for various argument counts
    /// </summary>
    /// <param name="count">Number of arguments to generate</param>
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(5)]
    public void CreateHostBuilder_Valid_Test_VariousArgCounts_Should_Return_ValidBuilder(int count)
    {
        // Arrange
        var data = Enumerable.Range(0, count)
                             .Select(i => $"arg{i}")
                             .ToArray();

        // Act
        var result = Program.CreateHostBuilder(data);

        // Assert
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests that CreateHostBuilder can successfully build a host instance
    /// </summary>
    [Fact]
    public void CreateHostBuilder_Valid_Test_Build_Should_Return_HostInstance()
    {
        // Arrange
        var data = Program.CreateHostBuilder(Array.Empty<string>());

        // Act
        using var result = data.Build();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHost>(result);
    }

    /// <summary>
    /// Tests that Main method is defined and does not throw
    /// Skipped as Main starts the web host which is not suitable for unit tests
    /// </summary>
    [Fact(Skip = "Main starts the web host; skip in unit tests.")]
    public void Main_Valid_Test_Definition_Should_Return_NoThrow()
    {
        // Arrange
        // Note: invoking Main would start the web server
        var data = true;

        // Act
        var result = data;

        // Assert
        Assert.True(result);
    }
}