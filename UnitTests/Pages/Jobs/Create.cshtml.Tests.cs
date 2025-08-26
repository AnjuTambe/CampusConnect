using System.Linq;
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
    public class CreateModelTests
    {
        private readonly Mock<ILogger<CreateModel>> _logger = new();
        private readonly Mock<IJobService> _service = new();

        // ????????????????? helper ?????????????????
        private CreateModel MakePage(out TempDataDictionary tempData)
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

            return new CreateModel(_logger.Object, _service.Object)
            {
                PageContext = pageCtx,
                TempData = tempData
            };
        }

        // ????????? constructor / OnGet basics ?????????
        [Fact]
        public void Ctor_Sets_NewJob_NotNull() =>
            Assert.NotNull(new CreateModel(_logger.Object, _service.Object).NewJob);

        [Theory]
        [InlineData("foo", "bar", 5)]
        [InlineData(null, null, 1)]
        public void OnGet_Preserves_Query_Params(string? s, string? l, int p)
        {
            var page = new CreateModel(_logger.Object, _service.Object);
            page.OnGet(s, l, p);
            Assert.Equal(s, page.SearchTerm);
            Assert.Equal(l, page.LocationFilter);
            Assert.Equal(p, page.CurrentPage);
        }

        // ????????? null-guard ?????????
        [Fact]
        public void OnPost_NullJob_Rerenders_WithModelError()
        {
            var page = MakePage(out _);
            page.NewJob = null!;
            var result = page.OnPost();
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Empty(_service.Invocations);
        }

        // ????????? initial ModelState invalid ?????????
        [Fact]
        public void OnPost_ModelStateInvalid_Rerenders()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing();
            page.ModelState.AddModelError("NewJob.Title", "err");
            var result = page.OnPost();
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Empty(_service.Invocations);
        }

        // ????????? special-case VALID shortcut success ?????????
        [Fact]
        public void OnPost_ValidShortcut_Redirects_AndCallsService()
        {
            var page = MakePage(out _);
            var job = new JobListing
            {
                Title = "Valid Job Title",
                EmployerName = "Valid Employer",
                Description = "d",
                Location = "Remote",
                ApplyUrl = "https://ok"
            };
            page.NewJob = job;
            _service.Setup(s => s.AddJob(job)).Callback<JobListing>(j => j.Id = "ID1");

            var result = page.OnPost();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.Single(_service.Invocations);
        }

        // ????????? special-case VALID shortcut *catch* branch ?????????
        [Fact]
        public void OnPost_ValidShortcut_ServiceThrows_Rerenders_WithError()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = "Valid Job Title",
                EmployerName = "Valid Employer",
                Description = "d",
                Location = "Remote",
                ApplyUrl = "https://ok"
            };
            _service.Setup(s => s.AddJob(It.IsAny<JobListing>()))
                    .Throws(new System.InvalidOperationException());

            var result = page.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Single(_service.Invocations);
        }

        // ????????? special-case INVALID shortcut ?????????
        [Fact]
        public void OnPost_InvalidShortcut_GivesFiveErrors()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = null,
                EmployerName = "",
                Description = null,
                Location = "",
                ApplyUrl = "invalid-url"
            };
            var result = page.OnPost();
            Assert.IsType<PageResult>(result);
            Assert.Equal(5, page.ModelState.ErrorCount);
        }

        // ????????? NEW: ApplyUrl starts with “invalid-” branch ?????????
        [Fact]
        public void OnPost_ApplyUrlStartsWithInvalid_Should_AddError_And_Rerender()
        {
            // Arrange
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = "Good",
                EmployerName = "Emp",
                Description = "D",
                Location = "Remote",
                ApplyUrl = "invalid-https://example.com"
            };

            // Act
            var result = page.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Contains("NewJob.ApplyUrl", page.ModelState.Keys);
            Assert.Empty(_service.Invocations);
        }

        // ????????? ApplyUrl regex failure (else-branch) ?????????
        [Fact]
        public void OnPost_BadApplyUrl_RegexBranch_Rerenders()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = "T",
                EmployerName = "E",
                Description = "D",
                Location = "Remote",
                ApplyUrl = "http://bad url"
            };
            var result = page.OnPost();
            Assert.IsType<PageResult>(result);
            Assert.Contains("NewJob.ApplyUrl", page.ModelState.Keys);
        }

        // ????????? location regex failure ?????????
        [Fact]
        public void OnPost_BadLocation_Rerenders_WithError()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = "T",
                EmployerName = "E",
                Description = "D",
                Location = "Weird Place",
                ApplyUrl = "https://ok"
            };
            var result = page.OnPost();
            Assert.IsType<PageResult>(result);
            Assert.Contains("NewJob.Location", page.ModelState.Keys);
        }

        // ????????? normal success ?????????
        [Fact]
        public void OnPost_NormalSuccess_RedirectsToDetails()
        {
            var page = MakePage(out var tempData);
            var job = new JobListing
            {
                Title = "Other",
                EmployerName = "Emp",
                Description = "d",
                Location = "Seattle, WA",
                ApplyUrl = "https://ok"
            };
            page.NewJob = job;
            _service.Setup(s => s.AddJob(job)).Callback<JobListing>(j => j.Id = "XYZ");

            var result = page.OnPost();

            Assert.Single(_service.Invocations);
            Assert.Equal("Job 'Other' created successfully.", tempData["Message"]);
        }

        // ????????? normal path, service throws ?????????
        [Fact]
        public void OnPost_NormalPath_ServiceThrows_Rerenders()
        {
            var page = MakePage(out _);
            page.NewJob = new JobListing
            {
                Title = "Other",
                EmployerName = "Emp",
                Description = "d",
                Location = "Seattle, WA",
                ApplyUrl = "https://ok"
            };
            _service.Setup(s => s.AddJob(It.IsAny<JobListing>()))
                    .Throws(new System.InvalidOperationException());

            var result = page.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.False(page.ModelState.IsValid);
            Assert.Single(_service.Invocations);
        }
    }
}