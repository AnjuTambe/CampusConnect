using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Moq;
using Xunit;

namespace UnitTests.Services.Interfaces;

/// <summary>
/// Concrete implementation of IJobService for testing interface coverage
/// </summary>
public class TestJobService : IJobService
{
    private readonly List<JobListing> _jobs = [];

    public IEnumerable<JobListing> GetJobs(string? searchTerm = null, string? location = null, string? jobType = null, string? employerNameFilter = null)
    {
        var result = _jobs.AsEnumerable();
        
        if (!string.IsNullOrEmpty(searchTerm))
            result = result.Where(j => j.Title?.Contains(searchTerm) ?? false);
        
        if (!string.IsNullOrEmpty(location))
            result = result.Where(j => j.Location == location);
        
        if (!string.IsNullOrEmpty(jobType))
            result = result.Where(j => j.JobType == jobType);
        
        if (!string.IsNullOrEmpty(employerNameFilter))
            result = result.Where(j => j.EmployerName == employerNameFilter);
        
        return result;
    }

    public JobListing? GetJobById(string id) => 
        _jobs.FirstOrDefault(j => j.Id == id);

    public void AddJob(JobListing newJob)
    {
        if (newJob is not null)
        {
            newJob.Id = Guid.NewGuid().ToString();
            _jobs.Add(newJob);
        }
    }

    public void UpdateJob(JobListing updatedJob)
    {
        if (updatedJob is not null && !string.IsNullOrEmpty(updatedJob.Id))
        {
            var index = _jobs.FindIndex(j => j.Id == updatedJob.Id);
            if (index >= 0)
            {
                _jobs[index] = updatedJob;
            }
        }
    }

    public void DeleteJob(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            _jobs.RemoveAll(j => j.Id == id);
        }
    }
}

/// <summary>
/// Unit tests for IJobService interface to verify contract and implementation behavior
/// </summary>
public class IJobServiceTests
{
    #region Interface Definition Tests

    /// <summary>
    /// Tests that interface is public and in correct namespace
    /// </summary>
    [Fact]
    public void Interface_IsPublicInterface_ValidInterface_ChecksCorrectly_IsPublicInterface()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data;
        
        // Assert
        Assert.True(result.IsInterface);
        Assert.True(result.IsPublic);
        Assert.Equal("CampusConnect.WebSite.Services", result.Namespace);
    }

    /// <summary>
    /// Tests that interface has exactly five methods with correct names
    /// </summary>
    [Fact]
    public void Interface_HasExactlyFiveMethods_ValidInterface_ChecksCorrectly_HasExactlyFiveMethods()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethods();
        
        // Assert
        Assert.Equal(5, result.Length);
        var methodNames = result.Select(m => m.Name).ToList();
        Assert.Contains("GetJobs", methodNames);
        Assert.Contains("GetJobById", methodNames);
        Assert.Contains("AddJob", methodNames);
        Assert.Contains("UpdateJob", methodNames);
        Assert.Contains("DeleteJob", methodNames);
    }

    /// <summary>
    /// Tests that interface is in correct assembly with expected name parts
    /// </summary>
    [Theory]
    [InlineData("CampusConnect")]
    [InlineData("WebSite")]
    public void Interface_IsInCorrectAssembly_ValidInterface_ChecksCorrectly_IsInCorrectAssembly(string expectedNamePart)
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.Assembly;
        
        // Assert
        Assert.NotNull(result);
        Assert.Contains(expectedNamePart, result.FullName);
    }

    /// <summary>
    /// Tests that all interface methods are public, abstract, and non-static
    /// </summary>
    [Fact]
    public void Interface_MethodsArePublic_ValidInterface_ChecksCorrectly_MethodsArePublic()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethods();
        
        // Assert
        foreach (var method in result)
        {
            Assert.True(method.IsPublic);
            Assert.True(method.IsAbstract); // Interface methods are abstract
            Assert.False(method.IsStatic);
        }
    }

    #endregion

    #region Method Signature Tests

    /// <summary>
    /// Tests that GetJobs method has correct signature with proper parameters and return type
    /// </summary>
    [Fact]
    public void GetJobs_HasCorrectSignature_ValidInterface_ChecksCorrectly_HasCorrectSignature()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethod("GetJobs");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(IEnumerable<JobListing>), result.ReturnType);
        
        // Check parameters
        var parameters = result.GetParameters();
        Assert.Equal(4, parameters.Length);
        
        Assert.Equal("searchTerm", parameters[0].Name);
        Assert.Equal(typeof(string), parameters[0].ParameterType);
        Assert.True(parameters[0].HasDefaultValue);
        Assert.Null(parameters[0].DefaultValue);
        
        Assert.Equal("location", parameters[1].Name);
        Assert.Equal(typeof(string), parameters[1].ParameterType);
        Assert.True(parameters[1].HasDefaultValue);
        Assert.Null(parameters[1].DefaultValue);
        
        Assert.Equal("jobType", parameters[2].Name);
        Assert.Equal(typeof(string), parameters[2].ParameterType);
        Assert.True(parameters[2].HasDefaultValue);
        Assert.Null(parameters[2].DefaultValue);
        
        Assert.Equal("employerNameFilter", parameters[3].Name);
        Assert.Equal(typeof(string), parameters[3].ParameterType);
        Assert.True(parameters[3].HasDefaultValue);
        Assert.Null(parameters[3].DefaultValue);
    }

    /// <summary>
    /// Tests that GetJobById method has correct signature with proper parameters and return type
    /// </summary>
    [Fact]
    public void GetJobById_HasCorrectSignature_ValidInterface_ChecksCorrectly_HasCorrectSignature()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethod("GetJobById");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(JobListing), result.ReturnType);
        
        // Check parameters
        var parameters = result.GetParameters();
        Assert.Single(parameters);
        Assert.Equal("id", parameters[0].Name);
        Assert.Equal(typeof(string), parameters[0].ParameterType);
        Assert.False(parameters[0].HasDefaultValue);
    }

    /// <summary>
    /// Tests that AddJob method has correct signature with proper parameters and return type
    /// </summary>
    [Fact]
    public void AddJob_HasCorrectSignature_ValidInterface_ChecksCorrectly_HasCorrectSignature()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethod("AddJob");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(void), result.ReturnType);
        
        // Check parameters
        var parameters = result.GetParameters();
        Assert.Single(parameters);
        Assert.Equal("newJob", parameters[0].Name);
        Assert.Equal(typeof(JobListing), parameters[0].ParameterType);
        Assert.False(parameters[0].HasDefaultValue);
    }

    /// <summary>
    /// Tests that UpdateJob method has correct signature with proper parameters and return type
    /// </summary>
    [Fact]
    public void UpdateJob_HasCorrectSignature_ValidInterface_ChecksCorrectly_HasCorrectSignature()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethod("UpdateJob");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(void), result.ReturnType);
        
        // Check parameters
        var parameters = result.GetParameters();
        Assert.Single(parameters);
        Assert.Equal("updatedJob", parameters[0].Name);
        Assert.Equal(typeof(JobListing), parameters[0].ParameterType);
        Assert.False(parameters[0].HasDefaultValue);
    }

    /// <summary>
    /// Tests that DeleteJob method has correct signature with proper parameters and return type
    /// </summary>
    [Fact]
    public void DeleteJob_HasCorrectSignature_ValidInterface_ChecksCorrectly_HasCorrectSignature()
    {
        // Arrange
        var data = typeof(IJobService);
        
        // Act
        var result = data.GetMethod("DeleteJob");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(void), result.ReturnType);
        
        // Check parameters
        var parameters = result.GetParameters();
        Assert.Single(parameters);
        Assert.Equal("id", parameters[0].Name);
        Assert.Equal(typeof(string), parameters[0].ParameterType);
        Assert.False(parameters[0].HasDefaultValue);
    }

    #endregion

    #region Implementation Tests

    /// <summary>
    /// Tests that TestJobService properly implements IJobService interface
    /// </summary>
    [Fact]
    public void TestJobService_ImplementsIJobService_ValidImplementation_ChecksCorrectly_ImplementsIJobService()
    {
        // Arrange
        var data = new TestJobService();
        
        // Act
        var result = data;
        
        // Assert
        Assert.IsAssignableFrom<IJobService>(result);
    }

    #endregion

    #region GetJobs Tests

    /// <summary>
    /// Tests that GetJobs properly filters results based on various search criteria
    /// </summary>
    [Theory]
    [InlineData(null, null, null, null, 2)]
    [InlineData("Developer", null, null, null, 1)]
    [InlineData(null, "Seattle", null, null, 1)]
    [InlineData(null, null, "Full-time", null, 1)]
    [InlineData(null, null, null, "TechCorp", 1)]
    [InlineData("Developer", "Seattle", "Full-time", "TechCorp", 1)]
    public void GetJobs_WithVariousFilters_ValidFilters_FiltersCorrectly_ReturnsFilteredResults(
        string? searchTerm, string? location, string? jobType, string? employerName, int expectedCount)
    {
        // Arrange
        var data = new TestJobService();
        var job1 = new JobListing 
        { 
            Title = "Software Developer", 
            Location = "Seattle", 
            JobType = "Full-time",
            EmployerName = "TechCorp"
        };
        var job2 = new JobListing 
        { 
            Title = "Data Analyst", 
            Location = "Portland", 
            JobType = "Part-time",
            EmployerName = "DataCo"
        };
        
        data.AddJob(job1);
        data.AddJob(job2);
        
        // Act
        var result = data.GetJobs(searchTerm, location, jobType, employerName);
        
        // Assert
        Assert.Equal(expectedCount, result.Count());
    }

    /// <summary>
    /// Tests that GetJobs handles null titles safely during filtering
    /// </summary>
    [Fact]
    public void GetJobs_WithNullTitle_NullValue_HandlesGracefully_HandlesNullSafely()
    {
        // Arrange
        var data = new TestJobService();
        var job1 = new JobListing 
        { 
            Title = null, // Null title to test null-coalescing operator
            Location = "Seattle", 
            JobType = "Full-time",
            EmployerName = "TechCorp"
        };
        var job2 = new JobListing 
        { 
            Title = "Developer", 
            Location = "Portland", 
            JobType = "Part-time",
            EmployerName = "DataCo"
        };
        
        data.AddJob(job1);
        data.AddJob(job2);
        
        // Act - Search for "Dev" which should only match job2, not job1 with null title
        var result = data.GetJobs(searchTerm: "Dev");
        
        // Assert
        Assert.Single(result);
        Assert.Equal("Developer", result.First().Title);
        
        // Also test that we can retrieve all jobs including the one with null title
        var allJobs = data.GetJobs();
        Assert.Equal(2, allJobs.Count());
    }

    #endregion

    #region GetJobById Tests

    /// <summary>
    /// Tests that GetJobById returns correct job when valid ID is provided
    /// </summary>
    [Fact]
    public void GetJobById_WithValidId_ValidId_FindsCorrectly_ReturnsCorrectJob()
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Test Job" };
        data.AddJob(job);
        
        // Get the ID that was assigned
        var addedJob = data.GetJobs().First();
        
        // Act
        var result = data.GetJobById(addedJob.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(addedJob.Id, result.Id);
        Assert.Equal("Test Job", result.Title);
    }

    /// <summary>
    /// Tests that GetJobById returns null when invalid ID is provided
    /// </summary>
    [Theory]
    [InlineData("non-existent")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void GetJobById_WithInvalidId_InvalidId_HandlesGracefully_ReturnsNull(string invalidId)
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Test Job" };
        data.AddJob(job);
        
        // Act
        var result = data.GetJobById(invalidId);
        
        // Assert
        Assert.Null(result);
    }

    #endregion

    #region AddJob Tests

    /// <summary>
    /// Tests that AddJob successfully adds valid job to collection
    /// </summary>
    [Fact]
    public void AddJob_WithValidJob_ValidJob_AddsSuccessfully_AddsToCollection()
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "New Job" };
        
        // Act
        data.AddJob(job);
        
        // Assert
        var result = data.GetJobs();
        Assert.Single(result);
        Assert.Equal("New Job", result.First().Title);
        Assert.NotNull(result.First().Id);
    }

    /// <summary>
    /// Tests that AddJob handles null job gracefully without throwing exception
    /// </summary>
    [Fact]
    public void AddJob_WithNullJob_NullInput_HandlesGracefully_DoesNotThrow()
    {
        // Arrange
        var data = new TestJobService();
        
        // Act & Assert
        var result = Record.Exception(() => data.AddJob(null!));
        Assert.Null(result);
        Assert.Empty(data.GetJobs());
    }

    #endregion

    #region UpdateJob Tests

    /// <summary>
    /// Tests that UpdateJob successfully updates existing job
    /// </summary>
    [Fact]
    public void UpdateJob_WithExistingJob_ValidJob_UpdatesSuccessfully_UpdatesJob()
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Original Title" };
        data.AddJob(job);
        
        var addedJob = data.GetJobs().First();
        var updatedJob = new JobListing 
        { 
            Id = addedJob.Id, 
            Title = "Updated Title" 
        };
        
        // Act
        data.UpdateJob(updatedJob);
        
        // Assert
        var result = data.GetJobById(addedJob.Id);
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
    }

    /// <summary>
    /// Tests that UpdateJob handles invalid job gracefully without throwing exception
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("non-existent")]
    public void UpdateJob_WithInvalidJob_InvalidInput_HandlesGracefully_DoesNotThrow(string? id)
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Original Job" };
        data.AddJob(job);
        
        JobListing? invalidJob = id is null 
            ? null 
            : new JobListing { Id = id, Title = "Test" };
        
        // Act
        var result = Record.Exception(() => data.UpdateJob(invalidJob!));
        
        // Assert
        Assert.Null(result);
        Assert.Single(data.GetJobs());
        Assert.Equal("Original Job", data.GetJobs().First().Title);
    }

    #endregion

    #region DeleteJob Tests

    /// <summary>
    /// Tests that DeleteJob successfully removes job with existing ID
    /// </summary>
    [Fact]
    public void DeleteJob_WithExistingId_ValidId_DeletesSuccessfully_RemovesJob()
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Job to Delete" };
        data.AddJob(job);
        
        var addedJob = data.GetJobs().First();
        
        // Act
        data.DeleteJob(addedJob.Id);
        
        // Assert
        var result = data.GetJobs();
        Assert.Empty(result);
    }

    /// <summary>
    /// Tests that DeleteJob handles invalid ID gracefully without throwing exception
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("non-existent")]
    public void DeleteJob_WithInvalidId_InvalidInput_HandlesGracefully_DoesNotThrow(string? invalidId)
    {
        // Arrange
        var data = new TestJobService();
        var job = new JobListing { Title = "Test Job" };
        data.AddJob(job);
        
        // Act
        var result = Record.Exception(() => data.DeleteJob(invalidId!));
        
        // Assert
        Assert.Null(result);
        Assert.Single(data.GetJobs());
    }

    #endregion
}