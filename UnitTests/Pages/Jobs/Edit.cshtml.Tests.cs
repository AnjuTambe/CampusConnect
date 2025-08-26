using System;
using System.Text.RegularExpressions;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Pages.Jobs;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Pages.Jobs
{
    /// <summary>
    /// Unit tests for EditModel.cshtml.cs – ensures every branch in OnGet() and OnPost() is exercised.
    /// </summary>
    public class EditModelTests
    {
        private readonly Mock<ILogger<EditModel>> _logger = new();
        private readonly Mock<IJobService> _service = new();

        /// <summary>
        /// A single JobListing instance that GetJobById("testId") will return.
        /// </summary>
        private readonly JobListing _testJob = new()
        {
            Id = "testId",
            Title = "Test Job",
            EmployerName = "Test Employer",
            Description = "Test Description",
            Location = "Seattle, WA",
            ApplyUrl = "https://example.com/apply",
            JobType = "Full-time"
        };

        public EditModelTests()
        {
            // Service returns _testJob when asked for "testId"; otherwise null.
            _service.Setup(s => s.GetJobById("testId")).Returns(_testJob);
            _service.Setup(s => s.GetJobById(It.Is<string>(id => id != "testId"))).Returns((JobListing?)null);
        }

        /// <summary>
        /// Factory for wiring up PageContext, TempData, and ModelState for EditModel.
        /// </summary>
        private EditModel MakePage(out TempDataDictionary tempData)
        {
            var http = new DefaultHttpContext();
            tempData = new TempDataDictionary(http, Mock.Of<ITempDataProvider>());
            var state = new ModelStateDictionary();
            var actCtx = new ActionContext(
                            http,
                            new Microsoft.AspNetCore.Routing.RouteData(),
                            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                            state);
            var pageCtx = new PageContext(actCtx)
            {
                ViewData = new ViewDataDictionary(
                               new EmptyModelMetadataProvider(), state)
            };

            return new EditModel(_logger.Object, _service.Object)
            {
                PageContext = pageCtx,
                TempData = tempData
            };
        }

        // ─────────────────────────────────────────────────────────────
        // OnGet Tests
        // ─────────────────────────────────────────────────────────────

        [Theory(DisplayName = "OnGet_NullOrEmptyId_ReturnsNotFound")]
        [InlineData(null)]
        [InlineData("")]
        public void OnGet_NullOrEmptyId_ReturnsNotFound(string id)
        {
            // Arrange
            var page = new EditModel(_logger.Object, _service.Object);

            // Act
            var result = page.OnGet(id!, "term", "loc", "emp", "3");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job ID cannot be empty.", notFound.Value);

            // Service should never be called
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnGet_NonexistentId_ReturnsNotFound")]
        public void OnGet_NonexistentId_ReturnsNotFound()
        {
            // Arrange: “bad” is not “testId”, so service returns null
            var page = new EditModel(_logger.Object, _service.Object);

            // Act
            var result = page.OnGet("bad", "any", "any", "any", "2");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job with ID 'bad' not found.", notFound.Value);

            // Exactly one call to GetJobById("bad")
            Assert.Single(_service.Invocations);
            Assert.Equal("bad", _service.Invocations[0].Arguments[0]);
        }

        [Fact(DisplayName = "OnGet_ValidId_PopulatesJobAndRendersPage")]
        public void OnGet_ValidId_PopulatesJobAndRendersPage()
        {
            // Arrange: service.GetJobById("testId") => _testJob
            var page = new EditModel(_logger.Object, _service.Object);

            // Act
            var result = page.OnGet("testId", "mySearch", "myLoc", "myEmp", "5");

            // Assert: returns PageResult
            Assert.IsType<PageResult>(result);

            // Bound properties set from query string
            Assert.Equal("mySearch", page.SearchTerm);
            Assert.Equal("myLoc", page.LocationFilter);
            Assert.Equal("myEmp", page.EmployerNameFilter);
            Assert.Equal(5, page.CurrentPage);

            // JobToUpdate was set to the mock job
            Assert.Same(_testJob, page.JobToUpdate);

            // Exactly one call to GetJobById("testId")
            Assert.Single(_service.Invocations);
            Assert.Equal("testId", _service.Invocations[0].Arguments[0]);
        }

        [Fact(DisplayName = "OnGet_InvalidCurrentPage_DefaultsTo1")]
        public void OnGet_InvalidCurrentPage_DefaultsTo1()
        {
            // Arrange: use a valid job ID but pass a non-numeric page
            var page = new EditModel(_logger.Object, _service.Object);

            // Act
            var result = page.OnGet("testId", "s", "l", "e", "notANumber");

            // Assert: still returns PageResult
            Assert.IsType<PageResult>(result);

            // CurrentPage must fall back to 1 when parsing fails
            Assert.Equal(1, page.CurrentPage);

            // JobToUpdate is populated normally
            Assert.Same(_testJob, page.JobToUpdate);
        }

        // ─────────────────────────────────────────────────────────────
        // OnPost Tests
        // ─────────────────────────────────────────────────────────────

        [Fact(DisplayName = "OnPost_InvalidModelState_ReturnsPageWithoutServiceCall")]
        public void OnPost_InvalidModelState_ReturnsPageWithoutServiceCall()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = _testJob;
            // Introduce a ModelState error so IsValid == false
            page.ModelState.AddModelError("JobToUpdate.Title", "Required");

            // Act
            var resultPage = page.OnPost();

            // Assert: rerenders
            Assert.IsType<PageResult>(resultPage);
            // Service.UpdateJob must not be invoked
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_InvalidApplyUrl_CustomValidation_RendersPageWithError")]
        public void OnPost_InvalidApplyUrl_CustomValidation_RendersPageWithError()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = new JobListing
            {
                Id = "x",
                Title = "T",
                EmployerName = "E",
                Description = "D",
                Location = "Remote",
                ApplyUrl = "http://invalid url"   // fails the regex
            };
            Assert.True(page.ModelState.IsValid);

            // Act
            var result = page.OnPost();

            // Assert: rerenders with model error on ApplyUrl
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Contains("JobToUpdate.ApplyUrl", page.ModelState.Keys);
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_InvalidLocation_CustomValidation_RendersPageWithError")]
        public void OnPost_InvalidLocation_CustomValidation_RendersPageWithError()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = new JobListing
            {
                Id = "x",
                Title = "T",
                EmployerName = "E",
                Description = "D",
                Location = "WrongFormat",    // fails the “City, ST” regex
                ApplyUrl = "https://ok"      // valid, so skip URL validation
            };
            Assert.True(page.ModelState.IsValid);

            // Act
            var result = page.OnPost();

            // Assert: rerenders with model error on Location
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Contains("JobToUpdate.Location", page.ModelState.Keys);
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_ServiceThrowsException_ReturnsPageWithModelError")]
        public void OnPost_ServiceThrowsException_ReturnsPageWithModelError()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = new JobListing
            {
                Id = "y",
                Title = "T",
                EmployerName = "E",
                Description = "D",
                Location = "Seattle, WA",
                ApplyUrl = "https://ok"
            };
            // Make UpdateJob(...) throw
            _service.Setup(s => s.UpdateJob(It.IsAny<JobListing>()))
                    .Throws(new InvalidOperationException("fail"));

            // Act
            var result = page.OnPost();

            // Assert: rerenders with model‐level error
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            // Exactly one invocation of UpdateJob was attempted
            Assert.Single(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_ValidUpdate_RedirectsWithRouteValues")]
        public void OnPost_ValidUpdate_RedirectsWithRouteValues()
        {
            // Arrange
            var page = MakePage(out var tempData);
            page.JobToUpdate = new JobListing
            {
                Id = "z",
                Title = "NewTitle",
                EmployerName = "NewEmployer",
                Description = "NewDesc",
                Location = "Seattle, WA",
                ApplyUrl = "https://ok"
            };
            page.SearchTerm = "find";
            page.LocationFilter = "loc";
            page.EmployerNameFilter = "emp";
            page.CurrentPage = 9;

            // Act
            var result = page.OnPost();

            // Assert: service was called exactly once with that same object
            Assert.Single(_service.Invocations);
            Assert.Same(page.JobToUpdate, _service.Invocations[0].Arguments[0]);

            // And the redirect parameters are correct
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowJobDetails", redirect.PageHandler);
            Assert.Equal("z", redirect.RouteValues["id"]);
            Assert.Equal("find", redirect.RouteValues["searchTerm"]);
            Assert.Equal("loc", redirect.RouteValues["locationFilter"]);
            Assert.Equal("emp", redirect.RouteValues["employerNameFilter"]);
            Assert.Equal(9, redirect.RouteValues["currentPage"]);
        }

        [Fact(DisplayName =
            "OnPost_EmptyApplyUrlAndEmptyLocation_Should_UpdateAndRedirect")]
        public void OnPost_EmptyApplyUrlAndEmptyLocation_Should_UpdateAndRedirect()
        {
            // Arrange
            var page = MakePage(out var tempData);
            page.JobToUpdate = new JobListing
            {
                Id = "w",
                Title = "NoValidation",
                EmployerName = "Emp",
                Description = "Desc",
                ApplyUrl = "",  // empty → skip URL custom‐validation
                Location = ""   // empty → skip Location custom‐validation
            };
            page.SearchTerm = "s1";
            page.LocationFilter = "l1";
            page.EmployerNameFilter = "e1";
            page.CurrentPage = 4;

            // Act
            var result = page.OnPost();

            // Assert: service was called exactly once with that same object
            Assert.Single(_service.Invocations);
            Assert.Same(page.JobToUpdate, _service.Invocations[0].Arguments[0]);

            // And the redirect is correct
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowJobDetails", redirect.PageHandler);
            Assert.Equal("w", redirect.RouteValues["id"]);
            Assert.Equal("s1", redirect.RouteValues["searchTerm"]);
            Assert.Equal("l1", redirect.RouteValues["locationFilter"]);
            Assert.Equal("e1", redirect.RouteValues["employerNameFilter"]);
            Assert.Equal(4, redirect.RouteValues["currentPage"]);
        }

        [Fact(DisplayName = "OnPost_ApplyUrlStartsWithInvalid_Should_AddError_And_Rerender")]
        public void OnPost_ApplyUrlStartsWithInvalid_Should_AddError_And_Rerender()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = new JobListing
            {
                Id = "badUrlCase",
                Title = "TitleBad",
                EmployerName = "EmployerBad",
                Description = "DescBad",
                // This triggers the StartsWith("invalid-") block
                ApplyUrl = "invalid-https://example.com/apply",
                // Valid location so only ApplyUrl is tested
                Location = "Seattle, WA"
            };
            Assert.True(page.ModelState.IsValid);

            // Act
            var result = page.OnPost();

            // Assert – re‐renders with a ModelState error on ApplyUrl
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Contains("JobToUpdate.ApplyUrl", page.ModelState.Keys);
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_ValidApplyUrl_EmptyLocation_Should_UpdateAndRedirect")]
        public void OnPost_ValidApplyUrl_EmptyLocation_Should_UpdateAndRedirect()
        {
            // Arrange
            var page = MakePage(out var tempData);
            page.JobToUpdate = new JobListing
            {
                Id = "m",
                Title = "MixedCase",
                EmployerName = "SomeEmployer",
                Description = "SomeDesc",
                ApplyUrl = "https://valid-url.com", // non-empty & matches regex
                Location = ""                       // empty → skip location validation
            };
            page.SearchTerm = "termX";
            page.LocationFilter = "locX";
            page.EmployerNameFilter = "empX";
            page.CurrentPage = 3;

            // Act
            var result = page.OnPost();

            // Assert: service.UpdateJob was called exactly once with the same object
            Assert.Single(_service.Invocations);
            Assert.Same(page.JobToUpdate, _service.Invocations[0].Arguments[0]);

            // Assert: redirect is correct
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowJobDetails", redirect.PageHandler);
            Assert.Equal("m", redirect.RouteValues["id"]);
            Assert.Equal("termX", redirect.RouteValues["searchTerm"]);
            Assert.Equal("locX", redirect.RouteValues["locationFilter"]);
            Assert.Equal("empX", redirect.RouteValues["employerNameFilter"]);
            Assert.Equal(3, redirect.RouteValues["currentPage"]);
        }

        [Fact(DisplayName = "OnPost_EmptyApplyUrl_ValidLocation_Should_UpdateAndRedirect")]
        public void OnPost_EmptyApplyUrl_ValidLocation_Should_UpdateAndRedirect()
        {
            // Arrange
            var page = MakePage(out var tempData);
            page.JobToUpdate = new JobListing
            {
                Id = "validCase",
                Title = "TitleHere",
                EmployerName = "SomeEmployer",
                Description = "SomeDescription",

                // EMPTY → skip URL‐regex block
                ApplyUrl = "",

                // NON‐EMPTY and valid format → skip Location‐regex error
                Location = "Seattle, WA"
            };
            page.SearchTerm = "searchValue";
            page.LocationFilter = "locValue";
            page.EmployerNameFilter = "empValue";
            page.CurrentPage = 7;

            Assert.True(page.ModelState.IsValid);

            // Act
            var result = page.OnPost();

            // Assert #1: UpdateJob was called exactly once with the same object
            Assert.Single(_service.Invocations);
            Assert.Same(page.JobToUpdate, _service.Invocations[0].Arguments[0]);

            // Assert #2: RedirectToPageResult with the correct route-values
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowJobDetails", redirect.PageHandler);
            Assert.Equal("validCase", redirect.RouteValues["id"]);
            Assert.Equal("searchValue", redirect.RouteValues["searchTerm"]);
            Assert.Equal("locValue", redirect.RouteValues["locationFilter"]);
            Assert.Equal("empValue", redirect.RouteValues["employerNameFilter"]);
            Assert.Equal(7, redirect.RouteValues["currentPage"]);
        }

        [Fact(DisplayName = "OnPost_ApplyUrlInvalid_And_LocationInvalid_Should_AddBothErrorsAndRerender")]
        public void OnPost_ApplyUrlInvalid_And_LocationInvalid_Should_AddBothErrorsAndRerender()
        {
            // Arrange
            var page = MakePage(out _);
            page.JobToUpdate = new JobListing
            {
                Id = "doubleBad",
                Title = "DoubleBad",
                EmployerName = "BadEmp",
                Description = "BadDesc",
                // Non‐empty but fails the URL regex:
                ApplyUrl = "http://bad url",
                // Non‐empty but fails the "City, ST" regex:
                Location = "WrongFormat"
            };
            Assert.True(page.ModelState.IsValid);

            // Act
            var result = page.OnPost();

            // Assert: we re‐render the page (PageResult), and BOTH ModelState errors appear
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);

            // There must be an error keyed on ApplyUrl and another on Location
            Assert.Contains("JobToUpdate.ApplyUrl", page.ModelState.Keys);
            Assert.Contains("JobToUpdate.Location", page.ModelState.Keys);

            // Since validation failed, UpdateJob should never have been called
            Assert.Empty(_service.Invocations);
        }

        [Fact(DisplayName = "OnPost_JobToUpdateNull_Should_AddModelError_And_Rerender")]
        public void OnPost_JobToUpdateNull_Should_AddModelError_And_Rerender()
        {
            // Arrange: leave JobToUpdate null
            var page = MakePage(out _);
            page.JobToUpdate = null;

            // Act: this will throw inside the try when calling JobToUpdate.ApplyUrl
            var result = page.OnPost();

            // Assert: we re‐render with a ModelState error keyed on the empty string
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Contains(string.Empty, page.ModelState.Keys);

            // Service.UpdateJob was never invoked
            Assert.Empty(_service.Invocations);
        }
    }
}