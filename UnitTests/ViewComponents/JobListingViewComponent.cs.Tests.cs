using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using CampusConnect.WebSite.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.ViewComponents
{
    /// <summary>
    /// Test class for JobListingViewComponent functionality
    /// Tests job listing display, filtering, and pagination behavior
    /// </summary>
    public class JobListingViewComponentTests
    {
        /// <summary>
        /// Mock job service for testing
        /// </summary>
        private readonly Mock<IJobService> _mockJobService = new();

        /// <summary>
        /// Mock logger for testing
        /// </summary>
        private readonly Mock<ILogger<JobListingViewComponent>> _mockLogger = new();

        /// <summary>
        /// Test data for job listings used across multiple tests
        /// </summary>
        private static readonly List<JobListing> TestJobs =
            new()
            {
                new()
                {
                    Id = "1",
                    Title = "Software Developer",
                    EmployerName = "TechCorp",
                    Description = "Development position",
                    Location = "Seattle, WA",
                    JobType = "Full-time",
                    ApplyUrl = "https://example.com/apply/1"
                },
                new()
                {
                    Id = "2",
                    Title = "UX Designer",
                    EmployerName = "DesignStudio",
                    Description = "Design interfaces",
                    Location = "Portland, OR",
                    JobType = "Part-time",
                    ApplyUrl = "https://example.com/apply/2"
                },
                new()
                {
                    Id = "3",
                    Title = "Data Analyst",
                    EmployerName = "TechAnalytics",
                    Description = "Data Analysis and reporting",
                    Location = "Seattle, WA",
                    JobType = "Full-time",
                    ApplyUrl = "https://example.com/apply/3"
                }
            };

        #region Test Data

        /// <summary>
        /// Test data for filter testing with various combinations
        /// </summary>
        public static IEnumerable<object[]> FilterTestData =>
            new[]
            {
                new object[]{ null, null,     null,         null,       new[] { "1", "2", "3" }, null,            null,            null },
                new object[]{ "Developer", null,         null,         null,       new[] { "1" },            "Developer",         null,            null },
                new object[]{ null, "Portland, OR",     null,         null,       new[] { "2" },            null,            "Portland, OR",   null },
                new object[]{ null, null,     "Part-time",  null,       new[] { "2" },            null,            null,           "Part-time" },
                new object[]{ null, null,     null,         "DesignStudio", new[] { "2" },        null,            null,            null },
                new object[]{ null, "Seattle, WA", "Full-time", null,       new[] { "1", "3" },      null,            "Seattle, WA",   "Full-time" }
            };

        #endregion

        #region Filter Tests

        /// <summary>
        /// Tests that InvokeAsync method applies filters correctly and returns expected results.
        /// </summary>
        /// <param name="searchTerm">Search term filter</param>
        /// <param name="locationFilter">Location filter</param>
        /// <param name="jobTypeFilter">Job type filter</param>
        /// <param name="employerNameFilter">Employer name filter</param>
        /// <param name="expectedIds">Expected job IDs in results</param>
        /// <param name="expectedSearchTerm">Expected search term in view model</param>
        /// <param name="expectedLocationFilter">Expected location filter in view model</param>
        /// <param name="expectedJobTypeFilter">Expected job type filter in view model</param>
        [Theory]
        [MemberData(nameof(FilterTestData))]
        public async Task InvokeAsync_Valid_Test_Filters_Should_Return_ExpectedResults(
            string? searchTerm,
            string? locationFilter,
            string? jobTypeFilter,
            string? employerNameFilter,
            string[] expectedIds,
            string? expectedSearchTerm,
            string? expectedLocationFilter,
            string? expectedJobTypeFilter)
        {
            // Arrange
            var data = TestJobs.Where(j => expectedIds.Contains(j.Id));
            _mockJobService
                .Setup(s => s.GetJobs(searchTerm, locationFilter, jobTypeFilter, employerNameFilter))
                .Returns(data);

            var component = new JobListingViewComponent(_mockJobService.Object, _mockLogger.Object);

            // Act
            var result = await component.InvokeAsync(searchTerm, locationFilter, jobTypeFilter, employerNameFilter, 1);

            // Assert
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<JobListingViewModel>(viewResult.ViewData.Model);

            Assert.Equal(expectedIds.Length, model.JobListings.Count());
            Assert.Equal(expectedSearchTerm, model.SearchTerm);
            Assert.Equal(expectedLocationFilter, model.LocationFilter);
            Assert.Equal(expectedJobTypeFilter, model.JobTypeFilter);
            Assert.Equal(employerNameFilter, model.EmployerNameFilter);
            Assert.Equal(expectedIds.Length, model.TotalItems);
            Assert.Equal(1, model.CurrentPage);
            Assert.Equal(1, model.TotalPages);
        }

        #endregion

        #region Pagination Tests

        /// <summary>
        /// Tests that InvokeAsync method defaults to first page for invalid page numbers
        /// </summary>
        /// <param name="requestedPage">Invalid page number to test</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10)]
        public async Task InvokeAsync_Invalid_Test_InvalidPage_Should_Return_DefaultsToFirstPage(int requestedPage)
        {
            // Arrange
            var data = TestJobs;
            _mockJobService
                .Setup(s => s.GetJobs(null, null, null, null))
                .Returns(data);

            var component = new JobListingViewComponent(_mockJobService.Object, _mockLogger.Object);

            // Act
            var result = await component.InvokeAsync(null, null, null, null, requestedPage);

            // Assert
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<JobListingViewModel>(viewResult.ViewData.Model);

            Assert.Equal(1, model.CurrentPage);
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Tests that InvokeAsync method returns empty model when no results found
        /// </summary>
        /// <param name="searchTerm">Search term that returns no results</param>
        [Theory]
        [InlineData("nonexistent")]
        [InlineData("no-results")]
        public async Task InvokeAsync_Valid_Test_NoResults_Should_Return_EmptyModel(string searchTerm)
        {
            // Arrange
            var data = Enumerable.Empty<JobListing>();
            _mockJobService
                .Setup(s => s.GetJobs(searchTerm, null, null, null))
                .Returns(data);

            var component = new JobListingViewComponent(_mockJobService.Object, _mockLogger.Object);

            // Act
            var result = await component.InvokeAsync(searchTerm, null, null, null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<JobListingViewModel>(viewResult.ViewData.Model);

            Assert.Empty(model.JobListings);
            Assert.Equal(0, model.TotalItems);
            Assert.Equal(0, model.TotalPages);
            Assert.Equal(1, model.CurrentPage);
        }

        /// <summary>
        /// Tests that InvokeAsync method treats null service response as empty
        /// </summary>
        [Fact]
        public async Task InvokeAsync_Valid_Test_ServiceReturnsNull_Should_Return_TreatedAsEmpty()
        {
            // Arrange
            IEnumerable<JobListing>? data = null;
            _mockJobService
                .Setup(s => s.GetJobs(null, null, null, null))
                .Returns(data!);

            var component = new JobListingViewComponent(_mockJobService.Object, _mockLogger.Object);

            // Act
            var result = await component.InvokeAsync(null, null, null, null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<JobListingViewModel>(viewResult.ViewData.Model);

            Assert.Empty(model.JobListings);
            Assert.Equal(0, model.TotalItems);
            Assert.Equal(0, model.TotalPages);
            Assert.Equal(1, model.CurrentPage);
        }

        #endregion

        #region New Test for Static FilterTestData Getter

        /// <summary>
        /// Verifies that FilterTestData returns exactly 6 test cases 
        /// and that first and last rows match the expected values.
        /// </summary>
        [Fact]
        public void FilterTestData_Getter_ReturnsExpectedTestCases_Expected()
        {
            // Act
            var data = FilterTestData.ToList();

            // Assert: exactly 6 rows
            Assert.Equal(6, data.Count);

            // First row is: { null, null, null, null, new[] { "1","2","3" }, null, null, null }
            Assert.Null((string?)data[0][0]);
            Assert.Null((string?)data[0][1]);
            Assert.Null((string?)data[0][2]);
            Assert.Null((string?)data[0][3]);
            Assert.Equal(new[] { "1", "2", "3" }, (string[])(data[0][4]!));
            Assert.Null((string?)data[0][5]);
            Assert.Null((string?)data[0][6]);
            Assert.Null((string?)data[0][7]);

            // Last row is: { null, "Seattle, WA", "Full-time", null, new[] { "1","3" }, null, "Seattle, WA", "Full-time" }
            Assert.Null((string?)data[5][0]);
            Assert.Equal("Seattle, WA", (string?)data[5][1]);
            Assert.Equal("Full-time", (string?)data[5][2]);
            Assert.Null((string?)data[5][3]);
            Assert.Equal(new[] { "1", "3" }, (string[])(data[5][4]!));
            Assert.Null((string?)data[5][5]);
            Assert.Equal("Seattle, WA", (string?)data[5][6]);
            Assert.Equal("Full-time", (string?)data[5][7]);
        }

        #endregion
    }
}