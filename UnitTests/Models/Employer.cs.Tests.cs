using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using CampusConnect.WebSite.Models;
using Xunit;

namespace UnitTests.Models;

/// <summary>
/// Test class for Employer model functionality
/// Tests validation rules and JSON serialization behavior
/// </summary>
public class EmployerTests
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
    /// Tests that a valid employer passes all validation rules
    /// </summary>
    [Fact]
    public void ValidEmployer_Valid_Test_AllProperties_Should_Return_PassValidation()
    {
        // Arrange
        var data = new Employer
        {
            EmployerName = "Test Company",
            CompanyVision = "Our vision",
            TechStack = ".NET, Azure, React"
        };

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
    [InlineData(nameof(Employer.EmployerName))]
    [InlineData(nameof(Employer.CompanyVision))]
    [InlineData(nameof(Employer.TechStack))]
    public void MissingRequiredProperty_Invalid_Test_NullProperty_Should_Return_FailValidation(string propertyName)
    {
        // Arrange
        var data = new Employer
        {
            EmployerName = "Name",
            CompanyVision = "Vision",
            TechStack = "Stack"
        };
        // Set the specified property to null
        typeof(Employer)
            .GetProperty(propertyName)!
            .SetValue(data, null);

        // Act
        var result = ValidateModel(data);

        // Assert
        Assert.Contains(result, r => r.MemberNames.Contains(propertyName));
    }

    /// <summary>
    /// Tests that JSON serialization uses correct property names
    /// </summary>
    /// <param name="clrProp">CLR property name</param>
    /// <param name="jsonName">Expected JSON property name</param>
    [Theory]
    [InlineData(nameof(Employer.EmployerName), "employer_name")]
    [InlineData(nameof(Employer.CompanyVision), "company_vision")]
    [InlineData(nameof(Employer.TechStack), "tech_stack")]
    public void JsonSerialization_Valid_Test_PropertyNames_Should_Return_CorrectNames(string clrProp, string jsonName)
    {
        // Arrange
        var data = new Employer
        {
            EmployerName = "Name",
            CompanyVision = "Vision",
            TechStack = "Stack"
        };

        // Act
        var result = JsonSerializer.Serialize(data);

        // Assert
        Assert.Contains($"\"{jsonName}\"", result);
    }
}