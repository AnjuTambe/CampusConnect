using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using CampusConnect.WebSite.Models;
using Xunit;

namespace UnitTests.Models;

/// <summary>
/// Test class for JobListing model functionality
/// Tests validation rules, JSON serialization, and ToString method
/// </summary>
public class JobListingTests
{
    /// <summary>
    /// Helper method to validate a model using data annotations
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <returns>List of validation results</returns>
    private static IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    /// <summary>
    /// Helper method to create a valid job listing for testing
    /// </summary>
    /// <returns>A valid JobListing instance</returns>
    private static JobListing CreateValidJobListing() => new()
    {
        Id = "1",
        Title = "Software Developer",
        EmployerName = "Tech Corp",
        Description = "Development position",
        Location = "Seattle, WA",
        ApplyUrl = "https://example.com/apply",
        DatePosted = new DateTime(2023, 1, 1),
        JobType = "Full-time"
    };

    /// <summary>
    /// Tests that a valid job listing passes all validation rules
    /// </summary>
    [Fact]
    public void ValidJob_Valid_Test_AllProperties_Should_Return_PassValidation()
    {
        // Arrange
        var data = CreateValidJobListing();
        
        // Act
        var result = ValidateModel(data);
        
        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    /// Tests that missing required properties fail validation
    /// </summary>
    /// <param name="propertyName">Name of the property to test</param>
    [Theory]
    [InlineData(nameof(JobListing.Title))]
    [InlineData(nameof(JobListing.EmployerName))]
    [InlineData(nameof(JobListing.Description))]
    [InlineData(nameof(JobListing.Location))]
    [InlineData(nameof(JobListing.ApplyUrl))]
    [InlineData(nameof(JobListing.JobType))]
    public void MissingRequiredProperty_Invalid_Test_NullProperty_Should_Return_FailValidation(string propertyName)
    {
        // Arrange
        var data = CreateValidJobListing();
        typeof(JobListing)
            .GetProperty(propertyName)!
            .SetValue(data, null);

        // Act
        var result = ValidateModel(data);
        
        // Assert
        Assert.Contains(result, r => r.MemberNames.Contains(propertyName));
    }

    /// <summary>
    /// Tests that invalid URLs fail validation
    /// </summary>
    /// <param name="url">Invalid URL to test</param>
    [Theory]
    [InlineData("invalid-url")]
    public void InvalidApplyUrl_Invalid_Test_BadUrl_Should_Return_FailValidation(string url)
    {
        // Arrange
        var data = CreateValidJobListing();
        data.ApplyUrl = url;

        // Act
        var result = ValidateModel(data);
        
        // Assert
        Assert.Contains(result, r => r.MemberNames.Contains(nameof(JobListing.ApplyUrl)));
    }

    /// <summary>
    /// Tests that ToString method returns valid JSON serializable string
    /// </summary>
    [Fact]
    public void ToString_Valid_Test_Serialization_Should_Return_JsonString()
    {
        // Arrange
        var data = CreateValidJobListing();
        
        // Act
        var result = data.ToString();
        var deserialized = JsonSerializer.Deserialize<JobListing>(result);
        
        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(data.Id, deserialized.Id);
        Assert.Equal(data.Title, deserialized.Title);
    }

    /// <summary>
    /// Test data for JSON property name validation
    /// </summary>
    public static IEnumerable<object[]> JsonPropertyData => new[]
    {
        new object[] { nameof(JobListing.Id),           "id"           },
        new object[] { nameof(JobListing.Title),        "title"        },
        new object[] { nameof(JobListing.EmployerName), "employer_name"},
        new object[] { nameof(JobListing.Description),  "description"  },
        new object[] { nameof(JobListing.Location),     "location"     },
        new object[] { nameof(JobListing.ApplyUrl),     "apply_url"    },
        new object[] { nameof(JobListing.DatePosted),   "date_posted"  },
        new object[] { nameof(JobListing.JobType),      "job_type"     }
    };

    /// <summary>
    /// Tests that JSON serialization uses correct property names
    /// </summary>
    /// <param name="clrName">CLR property name</param>
    /// <param name="jsonName">Expected JSON property name</param>
    [Theory]
    [MemberData(nameof(JsonPropertyData))]
    public void JsonSerialization_Valid_Test_PropertyNames_Should_Return_CorrectNames(string clrName, string jsonName)
    {
        // Arrange
        var data = CreateValidJobListing();
        
        // Act
        var result = JsonSerializer.Serialize(data);
        
        // Assert
        Assert.Contains($"\"{jsonName}\"", result);
    }

    /// <summary>
    /// Tests that JsonPropertyData has valid test entries
    /// </summary>
    [Fact]
    public void JsonPropertyData_Valid_Test_Entries_Should_Return_ValidData()
    {
        // Arrange
        var data = JsonPropertyData.ToList();

        // Act
        var result = data;

        // Assert
        Assert.True(result.Count > 0);
        foreach (var entry in result)
        {
            Assert.Equal(2, entry.Length);
            Assert.IsType<string>(entry[0]);
            Assert.IsType<string>(entry[1]);
            Assert.False(string.IsNullOrWhiteSpace((string)entry[0]));
            Assert.False(string.IsNullOrWhiteSpace((string)entry[1]));
        }
    }
}