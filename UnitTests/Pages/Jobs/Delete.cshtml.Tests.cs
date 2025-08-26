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
using System;
using Xunit;

namespace UnitTests.Pages.Jobs
{
    /// <summary>
    /// Unit tests for the DeleteModel page in Jobs section
    /// </summary>
    public class DeleteModelTests
    {
        private readonly Mock<ILogger<DeleteModel>> _logger;
        private readonly Mock<IJobService> _service;
        private readonly JobListing _job;

        public DeleteModelTests()
        {
            _logger = new Mock<ILogger<DeleteModel>>();
            _service = new Mock<IJobService>();
            _job = new JobListing
            {
                Id = "1",
                Title = "Software Engineer",
                EmployerName = "Contoso",
                Description = "Build things",
                Location = "Seattle, WA",
                JobType = "Full-time",
                ApplyUrl = "https://contoso.com/apply"
            };
        }

        /// <summary>
        /// Creates a DeleteModel page instance with necessary context for testing
        /// </summary>
        /// <param name="tempData">Output parameter for TempDataDictionary</param>
        /// <returns>A DeleteModel instance ready for testing</returns>
        private DeleteModel CreatePage(out TempDataDictionary tempData)
        {
            var http = new DefaultHttpContext();
            tempData = new TempDataDictionary(http, Mock.Of<ITempDataProvider>());
            var ctx = new PageContext(
                new ActionContext(
                    http,
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                    new ModelStateDictionary()))
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };
            return new DeleteModel(_logger.Object, _service.Object)
            {
                PageContext = ctx,
                TempData = tempData
            };
        }

        // ---------- OnGet Tests ----------
        /// <summary>
        /// Tests that OnGet returns NotFound when job ID is null or empty
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnGet_NullOrEmptyId_ReturnsNotFound_WhenCalled_ShouldReturnNotFoundObjectResult(string id)
        {
            // Arrange
            var data = CreatePage(out _);
            
            // Act
            var result = data.OnGet(id, "search", "loc", "emp", "2");
            
            // Assert
            var nf = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job ID cannot be empty for deletion confirmation.", nf.Value);
        }

        [Fact]
        public void OnGet_JobNotFound_ReturnsNotFound()
        {
            _service.Setup(s => s.GetJobById("404")).Returns((JobListing?)null);
            var page = CreatePage(out _);
            var result = page.OnGet("404", "s", "l", "e", "3");
            var nf = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job with ID '404' not found.", nf.Value);
        }

        [Fact]
        public void OnGet_ValidId_ReturnsPageAndSetsProperties()
        {
            _service.Setup(s => s.GetJobById("1")).Returns(_job);
            var page = CreatePage(out _);
            var result = page.OnGet("1", "sTerm", "lFilter", "eFilter", "5");
            Assert.IsType<PageResult>(result);
            Assert.Equal(_job, page.JobToDelete);
            Assert.Equal("sTerm", page.SearchTerm);
            Assert.Equal("lFilter", page.LocationFilter);
            Assert.Equal("eFilter", page.EmployerNameFilter);
            Assert.Equal(5, page.CurrentPage);
        }

        [Fact]
        public void OnGet_InvalidPageParameter_DefaultsToOne()
        {
            _service.Setup(s => s.GetJobById("1")).Returns(_job);
            var page = CreatePage(out _);
            var result = page.OnGet("1", null, null, null, "notint");
            Assert.IsType<PageResult>(result);
            Assert.Equal(1, page.CurrentPage);
        }

        // ---------- OnPost Tests ----------
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnPost_NullOrEmptyId_RedirectsToIndex(string id)
        {
            var page = CreatePage(out _);
            var result = page.OnPost(id!);
            var redir = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./Index", redir.PageName);
        }

        [Fact]
        public void OnPost_JobNotFound_ReturnsNotFound()
        {
            _service.Setup(s => s.GetJobById("404")).Returns((JobListing?)null);
            var page = CreatePage(out _);
            var result = page.OnPost("404");
            var nf = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job with ID '404' not found to delete.", nf.Value);
        }

        [Fact]
        public void OnPost_DeleteSuccessful_RedirectsToDashboardWithMessage()
        {
            _service.Setup(s => s.GetJobById("1")).Returns(_job);
            var page = CreatePage(out var tempData);
            var result = page.OnPost("1");
            var redir = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redir.PageName);
            Assert.Equal("Job 'Software Engineer' deleted successfully.", tempData["Message"]);
        }

        [Fact]
        public void OnPost_DeleteThrowsException_ReturnsPageWithError()
        {
            _service.Setup(s => s.GetJobById("boom")).Returns(_job);
            _service.Setup(s => s.DeleteJob("boom")).Throws(new InvalidOperationException());
            var page = CreatePage(out var tempData);
            var result = page.OnPost("boom");
            Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(string.Empty));
            Assert.True(tempData.ContainsKey("Error"));
            Assert.Equal("An error occurred while deleting the job.", tempData["Error"]);
        }

        [Fact]
        public void OnPost_GetJobByIdThrowsException_ReturnsPageWithError()
        {
            _service.Setup(s => s.GetJobById("ex")).Throws(new InvalidOperationException());
            var page = CreatePage(out var tempData);
            var result = page.OnPost("ex");
            Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(string.Empty));
            Assert.True(tempData.ContainsKey("Error"));
            Assert.Equal("An error occurred while deleting the job.", tempData["Error"]);
        }
    }
}