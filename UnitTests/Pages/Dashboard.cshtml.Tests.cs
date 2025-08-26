using System;
using System.Collections.Generic;
using System.Linq;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Pages;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the DashboardModel page
    /// </summary>
    public class DashboardModelTests
    {
        private readonly ILogger<DashboardModel> _logger = Mock.Of<ILogger<DashboardModel>>();
        private readonly Mock<IJobService> _jobService = new();
        private readonly Mock<IEmployerService> _employerService = new();

        public DashboardModelTests()
        {
            // Default Setup: test job service returns a couple of jobs
            _jobService
                .Setup(s => s.GetJobs(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(CreateTestJobs());

            // Default Setup: test employer service returns two employers
            _employerService
                .Setup(s => s.GetEmployers())
                .Returns(CreateTestEmployers());
        }

        /// <summary>
        /// Creates a DashboardModel instance for testing
        /// </summary>
        private DashboardModel CreateModel() => new(_logger, _jobService.Object, _employerService.Object);

        #region Constructor Tests

        /// <summary>
        /// Tests that the constructor initializes all properties correctly
        /// </summary>
        [Fact]
        public void Constructor_ValidDependencies_InitializesProperties_WhenCalled_ShouldSetDefaultValues()
        {
            // Arrange
            var model = CreateModel();

            // Act & Assert
            Assert.NotNull(model);
            Assert.Equal("Jobs", model.ActiveView);
            Assert.Equal(1, model.CurrentPage);
            Assert.False(model.ShowJobsList); // Default is false until OnGet is explicitly called
            Assert.NotNull(model.LocationOptions);
            Assert.NotNull(model.EmployerFilterOptions);
            Assert.NotNull(model.DistinctEmployerNames);
            Assert.NotNull(model.Employers);
        }

        #endregion

        #region OnGetShowJobs Tests

        public static IEnumerable<object?[]> ShowJobsTestData => new[]
        {
            new object?[] { null,         null,        null,      null,   1 },
            new object?[] { "Developer",  null,        null,      null,   1 },
            new object?[] { null,         "Seattle",   null,      null,   2 },
            new object?[] { null,         null,        "Full-time", null, 3 },
            new object?[] { null,         null,        null,      "TechCorp", 4 },
            new object?[] { "Dev",        "Sea",       "FT",      "EmpCo", 5 }
        };

        /// <summary>
        /// Tests that OnGetShowJobs sets all properties correctly with various parameter combinations
        /// </summary>
        [Theory]
        [MemberData(nameof(ShowJobsTestData))]
        public void OnGetShowJobs_VariousParameters_SetsPropertiesCorrectly_WhenCalled_ShouldUpdateAllFilters(
            string? search, string? location, string? jobType, string? employerName, int page)
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.OnGetShowJobs(search, location, jobType, employerName, page);

            // Assert
            Assert.True(model.ShowJobsList);
            Assert.Equal("Jobs", model.ActiveView);
            Assert.Equal(search, model.SearchTerm);
            Assert.Equal(location, model.LocationFilter);
            Assert.Equal(jobType, model.JobTypeFilter);
            Assert.Equal(employerName, model.EmployerNameFilter);
            Assert.Equal(page, model.CurrentPage);
        }

        #endregion

        #region OnGetShowJobDetails Tests

        /// <summary>
        /// Tests that OnGetShowJobDetails clears selected job for null or empty ID
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnGetShowJobDetails_Invalid_Test_NullOrEmptyId_Should_Return_ClearsSelectedJob(string? id)
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.OnGetShowJobDetails(id!, "search", "location", "jobType", "employer", 2);

            // Assert
            Assert.Null(model.SelectedJob);
            Assert.True(model.ShowJobsList);
            Assert.Equal("Jobs", model.ActiveView);
            Assert.Equal("search", model.SearchTerm);
            Assert.Equal("location", model.LocationFilter);
            Assert.Equal("jobType", model.JobTypeFilter);
            Assert.Equal("employer", model.EmployerNameFilter);
            Assert.Equal(2, model.CurrentPage);
        }

        /// <summary>
        /// Tests that OnGetShowJobDetails sets selected job and preserves state for valid ID
        /// </summary>
        [Fact]
        public void OnGetShowJobDetails_Valid_Test_ValidId_Should_Return_SetsSelectedJobAndPreservesState()
        {
            // Arrange
            var job = CreateTestJobs().First();
            _jobService.Setup(s => s.GetJobById(job.Id)).Returns(job);
            var model = CreateModel();

            // Act
            model.OnGetShowJobDetails(job.Id, "search", "location", "jobType", "employer", 3);

            // Assert
            Assert.Equal(job, model.SelectedJob);
            Assert.True(model.ShowJobsList);
            Assert.Equal("Jobs", model.ActiveView);
            Assert.Equal("search", model.SearchTerm);
            Assert.Equal("location", model.LocationFilter);
            Assert.Equal("jobType", model.JobTypeFilter);
            Assert.Equal("employer", model.EmployerNameFilter);
            Assert.Equal(3, model.CurrentPage);
        }

        /// <summary>
        /// Tests that OnGetShowJobDetails sets null selected job for non-existent ID
        /// </summary>
        [Fact]
        public void OnGetShowJobDetails_Invalid_Test_NonExistentId_Should_Return_SetsNullSelectedJob()
        {
            // Arrange
            var missingId = "nonexistent";
            _jobService.Setup(s => s.GetJobById(missingId)).Returns((JobListing?)null);
            var model = CreateModel();

            // Act
            model.OnGetShowJobDetails(missingId, "search", "location", "jobType", "employer", 1);

            // Assert
            Assert.Null(model.SelectedJob);
            Assert.True(model.ShowJobsList);
            Assert.Equal("Jobs", model.ActiveView);
        }

        #endregion

        #region OnGetShowEmployers Tests

        /// <summary>
        /// Tests that OnGetShowEmployers sets correct view state with no parameters
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_NoParams_Should_Return_SetsCorrectViewState()
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.OnGetShowEmployers();
            // Since we never cleared Employers explicitly, _employerService returns CreateTestEmployers()
            // DistinctEmployerNames will be populated from CreateTestEmployers()

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.NotNull(model.Employers);
            Assert.NotEmpty(model.DistinctEmployerNames);
        }

        /// <summary>
        /// Tests that OnGetShowEmployers sets correct state with various parameters
        /// </summary>
        [Theory]
        [InlineData("TechCorp", null, true)]
        [InlineData("UnknownCo", null, false)]
        [InlineData(null, "Tech", false)]
        [InlineData("", "Corp", false)]
        public void OnGetShowEmployers_Valid_Test_WithParameters_Should_Return_SetsCorrectState(
            string? employerId, string? searchTerm, bool shouldSelectEmployer)
        {
            // Arrange
            var model = CreateModel();

            // Reset any previous setups before customizing for this test
            _employerService.Reset();

            // By default, return CreateTestEmployers() unless overridden below
            _employerService
                .Setup(s => s.GetEmployers())
                .Returns(CreateTestEmployers());

            // If testing TechCorp selection, make sure that name exists
            if (employerId == "TechCorp")
            {
                _employerService
                    .Setup(s => s.GetEmployerByName("TechCorp"))
                    .Returns(CreateTestEmployers().First(e => e.EmployerName == "TechCorp"));
            }

            // For any other name that isn�t TechCorp, return null
            _employerService
                .Setup(s => s.GetEmployerByName(It.Is<string>(s => s != "TechCorp" || s == null)))
                .Returns((Employer)null);

            // If we expect no selection, return an empty list so DistinctEmployerNames ends up empty
            if (!shouldSelectEmployer)
            {
                _employerService
                    .Setup(s => s.GetEmployers())
                    .Returns(new List<Employer>());
            }

            // Act
            model.OnGetShowEmployers(employerId, searchTerm);

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.Equal(searchTerm, model.EmployerSearchTerm);

            if (shouldSelectEmployer)
            {
                Assert.NotNull(model.SelectedEmployer);
                Assert.Equal("TechCorp", model.SelectedEmployer!.EmployerName);
            }
            else
            {
                Assert.Null(model.SelectedEmployer);
            }
        }

        #endregion

        #region OnGetEmployerDetailsJson Tests

        public static IEnumerable<object?[]> EmployerJsonTestData => new[]
        {
            new object?[] { "TechCorp", 200, false },
            new object?[] { "UnknownCo", 404, true },
            new object?[] { "", 400, true },
            new object?[] { null, 400, true }
        };

        /// <summary>
        /// Tests that OnGetEmployerDetailsJson returns expected status code for various inputs
        /// </summary>
        [Theory]
        [MemberData(nameof(EmployerJsonTestData))]
        public void OnGetEmployerDetailsJson_Valid_Test_VariousInputs_Should_Return_ExpectedStatusCode(
            string? employerId, int expectedStatusCode, bool shouldContainError)
        {
            // Arrange
            var model = CreateModel();

            // If employerId == "TechCorp", ensure there is a matching Employer
            if (employerId == "TechCorp")
            {
                _employerService
                    .Setup(s => s.GetEmployers())
                    .Returns(new List<Employer>
                    {
                        new Employer { EmployerName = "TechCorp", CompanyVision = "V", TechStack = "S" }
                    });
            }
            else
            {
                _employerService
                    .Setup(s => s.GetEmployers())
                    .Returns(new List<Employer>());
            }

            // Act
            var result = model.OnGetEmployerDetailsJson(employerId!) as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedStatusCode, result!.StatusCode);

            // If we expect an error, check that the JSON object has an "error" property
            if (shouldContainError)
            {
                var prop = result.Value!.GetType().GetProperty("error");
                Assert.NotNull(prop);
            }
        }

        /// <summary>
        /// Tests OnGetShowEmployers with null employer service result
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_NullEmployerServiceResult_Should_Return_HandlesNullGracefully()
        {
            // Arrange
            var model = CreateModel();
            _employerService.Setup(s => s.GetEmployers()).Returns((List<Employer>)null!);

            // Act
            model.OnGetShowEmployers();

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.NotNull(model.Employers);
            Assert.Empty(model.DistinctEmployerNames);
        }

        /// <summary>
        /// Tests OnGetShowEmployers with empty employer list
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_EmptyEmployerList_Should_Return_HandlesEmptyListCorrectly()
        {
            // Arrange
            var model = CreateModel();
            _employerService.Setup(s => s.GetEmployers()).Returns(new List<Employer>());

            // Act
            model.OnGetShowEmployers();

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.NotNull(model.Employers);
            Assert.Empty(model.DistinctEmployerNames);
            Assert.Null(model.SelectedEmployer);
        }

        /// <summary>
        /// Tests OnGetShowEmployers with search term filtering
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_WithSearchTermFiltering_Should_Return_FiltersCorrectly()
        {
            // Arrange
            var model = CreateModel();
            var employers = new List<Employer>
            {
                new Employer { EmployerName = "TechCorp", CompanyVision = "V1", TechStack = "S1" },
                new Employer { EmployerName = "DataCorp", CompanyVision = "V2", TechStack = "S2" },
                new Employer { EmployerName = "WebCorp", CompanyVision = "V3", TechStack = "S3" }
            };
            _employerService.Setup(s => s.GetEmployers()).Returns(employers);

            // Act
            model.OnGetShowEmployers(null, "Tech");

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.Equal("Tech", model.EmployerSearchTerm);
            Assert.Single(model.DistinctEmployerNames);
            Assert.Contains("TechCorp", model.DistinctEmployerNames);
        }

        /// <summary>
        /// Tests OnGetShowEmployers with case-insensitive search
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_CaseInsensitiveSearch_Should_Return_FiltersCorrectly()
        {
            // Arrange
            var model = CreateModel();
            var employers = new List<Employer>
            {
                new Employer { EmployerName = "TechCorp", CompanyVision = "V1", TechStack = "S1" },
                new Employer { EmployerName = "DataCorp", CompanyVision = "V2", TechStack = "S2" }
            };
            _employerService.Setup(s => s.GetEmployers()).Returns(employers);

            // Act
            model.OnGetShowEmployers(null, "TECH");

            // Assert
            Assert.Equal("TECH", model.EmployerSearchTerm);
            Assert.Single(model.DistinctEmployerNames);
            Assert.Contains("TechCorp", model.DistinctEmployerNames);
        }

        /// <summary>
        /// Tests OnGetShowEmployers with employer selection by ID
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_WithEmployerIdSelection_Should_Return_SelectsCorrectEmployer()
        {
            // Arrange
            var model = CreateModel();
            var employers = CreateTestEmployers();
            _employerService.Setup(s => s.GetEmployers()).Returns(employers);

            // Act
            model.OnGetShowEmployers("TechCorp", null);

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.NotNull(model.SelectedEmployer);
            Assert.Equal("TechCorp", model.SelectedEmployer!.EmployerName);
        }

        /// <summary>
        /// Tests OnGetShowEmployers when no employer matches the ID - should select first from DistinctEmployerNames
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_NoEmployerMatchesId_Should_Return_SelectsFirstEmployer()
        {
            // Arrange
            var model = CreateModel();
            var employers = CreateTestEmployers();
            _employerService.Setup(s => s.GetEmployers()).Returns(employers);

            // Act
            model.OnGetShowEmployers("NonExistentCorp", null);

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            // When no employer matches the ID, it should select the first employer from DistinctEmployerNames
            // Since DistinctEmployerNames will contain both "TechCorp" and "OtherCo" ordered alphabetically,
            // "OtherCo" comes first alphabetically
            Assert.NotNull(model.SelectedEmployer);
            Assert.Equal("OtherCo", model.SelectedEmployer!.EmployerName); // First alphabetically
        }

        /// <summary>
        /// Tests OnGetShowEmployers when DistinctEmployerNames is empty
        /// </summary>
        [Fact]
        public void OnGetShowEmployers_Valid_Test_EmptyDistinctEmployerNames_Should_Return_NoEmployerSelected()
        {
            // Arrange
            var model = CreateModel();
            _employerService.Setup(s => s.GetEmployers()).Returns(new List<Employer>());

            // Act
            model.OnGetShowEmployers("SomeId", "NonMatchingSearch");

            // Assert
            Assert.False(model.ShowJobsList);
            Assert.Equal("Employers", model.ActiveView);
            Assert.Equal("NonMatchingSearch", model.EmployerSearchTerm);
            Assert.Empty(model.DistinctEmployerNames);
            Assert.Null(model.SelectedEmployer);
        }

        #endregion

        #region New Tests for Static TestData Properties

        /// <summary>
        /// Verifies that ShowJobsTestData getter returns exactly 6 rows, 
        /// and that its contents match the expected values.
        /// </summary>
        [Fact]
        public void ShowJobsTestData_Getter_ReturnsExpectedTestCases_Expected()
        {
            // Act
            var data = ShowJobsTestData.ToList();

            // Assert: there should be exactly 6 test rows
            Assert.Equal(6, data.Count);

            // Verify the first row is { null, null, null, null, 1 }
            Assert.Null((string?)data[0][0]);
            Assert.Null((string?)data[0][1]);
            Assert.Null((string?)data[0][2]);
            Assert.Null((string?)data[0][3]);
            Assert.Equal(1, (int)data[0][4]);

            // Verify the last row is { "Dev", "Sea", "FT", "EmpCo", 5 }
            Assert.Equal("Dev", (string?)data[5][0]);
            Assert.Equal("Sea", (string?)data[5][1]);
            Assert.Equal("FT", (string?)data[5][2]);
            Assert.Equal("EmpCo", (string?)data[5][3]);
            Assert.Equal(5, (int)data[5][4]);
        }

        /// <summary>
        /// Verifies that EmployerJsonTestData getter returns exactly 4 rows,
        /// and that the status codes line up as expected.
        /// </summary>
        [Fact]
        public void EmployerJsonTestData_Getter_ReturnsExpectedTestCases_Expected()
        {
            // Act
            var data = EmployerJsonTestData.ToList();

            // Assert: there should be exactly 4 test rows
            Assert.Equal(4, data.Count);

            // Row 0: { "TechCorp", 200, false }
            Assert.Equal("TechCorp", (string?)data[0][0]);
            Assert.Equal(200, (int)data[0][1]);
            Assert.False((bool)data[0][2]);

            // Row 3: { null, 400, true }
            Assert.Null((string?)data[3][0]);
            Assert.Equal(400, (int)data[3][1]);
            Assert.True((bool)data[3][2]);
        }

        #endregion

        #region OnGet Tests

        /// <summary>
        /// Tests that OnGet calls PopulateLocationOptions and PopulateEmployerFilterOptions
        /// </summary>
        [Fact]
        public void OnGet_Valid_Test_CallsPopulateMethods_Should_Return_PopulateMethodsCalled()
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.OnGet();

            // Assert
            Assert.NotNull(model.LocationOptions);
            Assert.NotNull(model.EmployerFilterOptions);
            // Verify that the job service was called to populate options
            _jobService.Verify(s => s.GetJobs(null, null, null, null), Times.AtLeast(2)); // Called twice: once for locations, once for employers
        }

        /// <summary>
        /// Tests OnGet when EmployerSearchTerm is provided and ActiveView is Employers
        /// </summary>
        [Fact]
        public void OnGet_Valid_Test_WithEmployerSearchTermAndEmployersView_Should_Return_CallsOnGetShowEmployers()
        {
            // Arrange
            var model = CreateModel();
            model.EmployerSearchTerm = "TechCorp";
            model.ActiveView = "Employers";

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("Employers", model.ActiveView);
            Assert.Equal("TechCorp", model.EmployerSearchTerm);
            Assert.False(model.ShowJobsList);
        }

        /// <summary>
        /// Tests OnGet when Request.Query is null
        /// </summary>
        [Fact]
        public void OnGet_Valid_Test_WithNullRequestQuery_Should_Return_SetsDefaultView()
        {
            // Arrange
            var model = CreateModel();
            // Request.Query will be null by default in unit tests

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("Jobs", model.ActiveView);
            Assert.True(model.ShowJobsList);
        }

        /// <summary>
        /// Tests OnGet when Request.Query["ActiveView"] is empty
        /// </summary>
        [Fact]
        public void OnGet_Valid_Test_WithEmptyActiveViewQuery_Should_Return_SetsDefaultView()
        {
            // Arrange
            var model = CreateModel();
            // Simulate empty ActiveView query parameter by setting up PageContext
            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            var queryCollection = new Microsoft.AspNetCore.Http.QueryCollection();
            httpContext.Request.Query = queryCollection;

            model.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
            {
                HttpContext = httpContext
            };

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("Jobs", model.ActiveView);
            Assert.True(model.ShowJobsList);
        }

        /// <summary>
        /// Tests OnGet when job filter parameters are present
        /// </summary>
        [Theory]
        [InlineData("search", null, null, null)]
        [InlineData(null, "location", null, null)]
        [InlineData(null, null, "jobType", null)]
        [InlineData(null, null, null, "employer")]
        public void OnGet_Valid_Test_WithJobFilterParameters_Should_Return_SetsJobsView(
            string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter)
        {
            // Arrange
            var model = CreateModel();
            model.SearchTerm = searchTerm;
            model.LocationFilter = locationFilter;
            model.JobTypeFilter = jobTypeFilter;
            model.EmployerNameFilter = employerNameFilter;

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("Jobs", model.ActiveView);
            Assert.True(model.ShowJobsList);
        }

        /// <summary>
        /// Tests OnGet when only EmployerSearchTerm is present (no job filters)
        /// </summary>
        [Fact]
        public void OnGet_Valid_Test_WithOnlyEmployerSearchTerm_Should_Return_SetsEmployersView()
        {
            // Arrange
            var model = CreateModel();
            model.EmployerSearchTerm = "TechCorp";
            model.ActiveView = "Jobs"; // Start with Jobs view

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("Employers", model.ActiveView);
            Assert.False(model.ShowJobsList);
        }

        /// <summary>
        /// Tests OnGet sets ShowJobsList based on ActiveView (final line 148 in OnGet)
        /// </summary>
        [Theory]
        [InlineData("Jobs", true)]
        [InlineData("Employers", false)]
        [InlineData("SomeOtherView", false)]
        public void OnGet_Valid_Test_SetsShowJobsListBasedOnActiveView_Should_Return_CorrectShowJobsList(
            string activeView, bool expectedShowJobsList)
        {
            // Arrange
            var model = CreateModel();
            model.ActiveView = activeView;

            // Set up a mock HttpContext with Request.Query that has ActiveView to avoid the default logic
            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            var queryCollection = new Microsoft.AspNetCore.Http.QueryCollection(
                new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
                {
                    ["ActiveView"] = activeView
                });
            httpContext.Request.Query = queryCollection;

            model.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
            {
                HttpContext = httpContext
            };

            // Act
            model.OnGet();

            // Assert - ShowJobsList is set based on ActiveView at line 148: ShowJobsList = (ActiveView == "Jobs");
            Assert.Equal(expectedShowJobsList, model.ShowJobsList);
        }

        #endregion

        #region HasAnyJobFilterParameters Tests

        /// <summary>
        /// Tests HasAnyJobFilterParameters returns false when all parameters are null or empty
        /// Note: string.IsNullOrEmpty() does NOT treat whitespace as empty, so "   " returns true
        /// </summary>
        [Theory]
        [InlineData(null, null, null, null)]
        [InlineData("", "", "", "")]
        public void HasAnyJobFilterParameters_Valid_Test_AllParametersEmpty_Should_Return_False(
            string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter)
        {
            // Arrange
            var model = CreateModel();
            model.SearchTerm = searchTerm;
            model.LocationFilter = locationFilter;
            model.JobTypeFilter = jobTypeFilter;
            model.EmployerNameFilter = employerNameFilter;

            // Act
            var result = model.GetType()
                .GetMethod("HasAnyJobFilterParameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(model, null);

            // Assert
            Assert.False((bool)result!);
        }

        /// <summary>
        /// Tests HasAnyJobFilterParameters returns true for whitespace strings (since string.IsNullOrEmpty treats them as non-empty)
        /// </summary>
        [Theory]
        [InlineData("   ", null, null, null)]
        [InlineData(null, "   ", null, null)]
        [InlineData(null, null, "   ", null)]
        [InlineData(null, null, null, "   ")]
        public void HasAnyJobFilterParameters_Valid_Test_WhitespaceParameters_Should_Return_True(
            string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter)
        {
            // Arrange
            var model = CreateModel();
            model.SearchTerm = searchTerm;
            model.LocationFilter = locationFilter;
            model.JobTypeFilter = jobTypeFilter;
            model.EmployerNameFilter = employerNameFilter;

            // Act
            var result = model.GetType()
                .GetMethod("HasAnyJobFilterParameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(model, null);

            // Assert
            Assert.True((bool)result!);
        }

        /// <summary>
        /// Tests HasAnyJobFilterParameters returns true when any parameter has a value
        /// </summary>
        [Theory]
        [InlineData("search", null, null, null)]
        [InlineData(null, "location", null, null)]
        [InlineData(null, null, "jobType", null)]
        [InlineData(null, null, null, "employer")]
        [InlineData("search", "location", "jobType", "employer")]
        public void HasAnyJobFilterParameters_Valid_Test_AnyParameterHasValue_Should_Return_True(
            string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter)
        {
            // Arrange
            var model = CreateModel();
            model.SearchTerm = searchTerm;
            model.LocationFilter = locationFilter;
            model.JobTypeFilter = jobTypeFilter;
            model.EmployerNameFilter = employerNameFilter;

            // Act
            var result = model.GetType()
                .GetMethod("HasAnyJobFilterParameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(model, null);

            // Assert
            Assert.True((bool)result!);
        }

        #endregion

        #region Test Data Helpers

        /// <summary>
        /// Helper method to create test job listings for testing
        /// </summary>
        private static IEnumerable<JobListing> CreateTestJobs() =>
            new List<JobListing>
            {
                new JobListing
                {
                    Id = "1", Title = "Dev", EmployerName = "Acme",
                    Description = "x", Location = "L", JobType = "T", ApplyUrl = "https://a"
                },
                new JobListing
                {
                    Id = "2", Title = "QA", EmployerName = "Beta",
                    Description = "y", Location = "M", JobType = "U", ApplyUrl = "https://b"
                }
            };

        /// <summary>
        /// Helper method to create test employers for testing
        /// </summary>
        private static List<Employer> CreateTestEmployers() =>
            new List<Employer>
            {
                new Employer { EmployerName = "TechCorp", CompanyVision = "V", TechStack = "S" },
                new Employer { EmployerName = "OtherCo",  CompanyVision = "W", TechStack = "T" }
            };

        #endregion
    }
}