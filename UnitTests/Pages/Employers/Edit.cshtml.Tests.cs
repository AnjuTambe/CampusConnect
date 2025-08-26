using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Pages.Employers;
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

namespace UnitTests.Pages.Employers
{
    /// <summary>
    /// Unit tests for the Edit page model for employers.
    /// </summary>
    public class EditModelTests
    {
        private readonly Mock<ILogger<EditModel>> _logger = new();
        private readonly Mock<IEmployerService> _employerService = new();
        private readonly Employer _testEmployer = new()
        {
            EmployerName = "Test Employer",
            CompanyVision = "Test Vision",
            TechStack = "Test Stack"
        };

        #region Setup

        /// <summary>
        /// Creates an EditModel page with TempData and mocks wired up.
        /// </summary>
        private EditModel CreatePage(out TempDataDictionary tempData)
        {
            var httpContext = new DefaultHttpContext();
            tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(
                httpContext,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                modelState);

            var pageContext = new PageContext(actionContext)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
            };

            var page = new EditModel(_logger.Object, _employerService.Object)
            {
                PageContext = pageContext,
                TempData = tempData
            };
            return page;
        }

        #endregion

        #region Constructor Tests

        /// <summary>
        /// Tests that the constructor initializes the current page to 1.
        /// </summary>
        [Fact]
        public void Constructor_ValidInput_InitializesProperties_CorrectValues_Expected()
        {
            // Arrange & Act
            var page = CreatePage(out _);

            // Assert
            Assert.NotNull(page);
            Assert.Equal(1, page.CurrentPage);
        }

        #endregion

        #region OnGet Tests

        /// <summary>
        /// Tests that OnGet returns NotFound when id is null or empty and preserves state.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnGet_NullOrEmptyId_ReturnsNotFound_ForInvalidInput_Expected(string? id)
        {
            // Arrange
            var page = CreatePage(out _);

            // Act
            var result = page.OnGet(id, "searchTerm", 2);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal("searchTerm", page.EmployerSearchTerm);
            Assert.Equal(2, page.CurrentPage);
        }

        /// <summary>
        /// Tests that OnGet returns NotFound when employer is not found.
        /// </summary>
        [Fact]
        public void OnGet_EmployerNotFound_ReturnsNotFound_WhenServiceReturnsNull_Expected()
        {
            // Arrange
            var page = CreatePage(out _);
            _employerService.Setup(s => s.GetEmployerByName("nonexistent"))
                            .Returns((Employer?)null);

            // Act
            var result = page.OnGet("nonexistent", "searchTerm", 3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests that OnGet returns PageResult and sets Employer_To_Update when valid.
        /// </summary>
        [Fact]
        public void OnGet_ValidId_ReturnsPageWithEmployer_WhenEmployerExists_Expected()
        {
            // Arrange
            var page = CreatePage(out _);
            _employerService.Setup(s => s.GetEmployerByName(_testEmployer.EmployerName))
                            .Returns(_testEmployer);

            // Act
            var result = page.OnGet(_testEmployer.EmployerName, "searchTerm", 4);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal(_testEmployer, page.Employer_To_Update);
            Assert.Equal("searchTerm", page.EmployerSearchTerm);
            Assert.Equal(4, page.CurrentPage);
        }

        #endregion

        #region OnPost Tests

        /// <summary>
        /// Tests that OnPost returns page with error when Employer_To_Update is null.
        /// </summary>
        [Fact]
        public void OnPost_NullEmployer_ReturnsPageWithError_WhenEmployerIsNull_Expected()
        {
            // Arrange
            var page = CreatePage(out _);
            page.Employer_To_Update = null!;

            // Act
            var result = page.OnPost();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(string.Empty));
            var error = Assert.Single(page.ModelState[string.Empty].Errors);
            Assert.Equal("An unexpected error occurred. Please try again.", error.ErrorMessage);
        }

        /// <summary>
        /// Tests that OnPost returns page with validation errors for missing fields.
        /// </summary>
        [Theory]
        [InlineData(null, "Vision", "Stack", "Employer_To_Update.EmployerName")]
        [InlineData("", "Vision", "Stack", "Employer_To_Update.EmployerName")]
        [InlineData("Name", null, "Stack", "Employer_To_Update.CompanyVision")]
        [InlineData("Name", "", "Stack", "Employer_To_Update.CompanyVision")]
        [InlineData("Name", "Vision", null, "Employer_To_Update.TechStack")]
        [InlineData("Name", "Vision", "", "Employer_To_Update.TechStack")]
        public void OnPost_MissingRequiredFields_ReturnsPageWithValidationErrors_ForIncompleteData_Expected(
            string? name, string? vision, string? stack, string expectedKey)
        {
            // Arrange
            var page = CreatePage(out _);
            page.Employer_To_Update = new Employer
            {
                EmployerName = name,
                CompanyVision = vision,
                TechStack = stack
            };

            // Act
            var result = page.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(expectedKey));
        }

        /// <summary>
        /// Tests that OnPost redirects with success when update succeeds.
        /// </summary>
        [Fact]
        public void OnPost_ValidEmployer_RedirectsToDashboardWithMessage_WhenUpdateSucceeds_Expected()
        {
            // Arrange
            var page = CreatePage(out var tempData);
            page.Employer_To_Update = new Employer
            {
                EmployerName = _testEmployer.EmployerName,
                CompanyVision = "Updated Vision",
                TechStack = "Updated Stack"
            };
            page.EmployerSearchTerm = "searchTerm";
            page.CurrentPage = 5;

            // Act
            var result = page.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowEmployers", redirect.PageHandler);
            Assert.NotNull(redirect.RouteValues);
            Assert.Equal("searchTerm", redirect.RouteValues["employerSearchTerm"]);
            Assert.Equal(5, redirect.RouteValues["currentPage"]);
            Assert.Equal($"Employer '{_testEmployer.EmployerName}' updated successfully.",
                         tempData["Message"]);

            _employerService.Verify(s => s.UpdateEmployer(It.IsAny<Employer>()), Times.Once);
        }

        /// <summary>
        /// Tests that OnPost returns page with error when service throws.
        /// </summary>
        [Fact]
        public void OnPost_ServiceThrowsException_ReturnsPageWithError_WhenUpdateFails_Expected()
        {
            // Arrange
            var page = CreatePage(out _);
            page.Employer_To_Update = new Employer
            {
                EmployerName = _testEmployer.EmployerName,
                CompanyVision = "Updated Vision",
                TechStack = "Updated Stack"
            };
            _employerService.Setup(s => s.UpdateEmployer(It.IsAny<Employer>()))
                            .Throws(new InvalidOperationException("Test exception"));

            // Act
            var result = page.OnPost();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(string.Empty));
            var error = Assert.Single(page.ModelState[string.Empty].Errors);
            Assert.Equal("An error occurred while updating the employer. Please try again.",
                         error.ErrorMessage);
        }

        /// <summary>
        /// Tests that OnPost returns page with error when employer not found.
        /// </summary>
        [Fact]
        public void OnPost_EmployerNotFound_ReturnsPageWithError_WhenEmployerDoesNotExist_Expected()
        {
            // Arrange
            var page = CreatePage(out _);
            page.Employer_To_Update = new Employer
            {
                EmployerName = "Nonexistent Employer",
                CompanyVision = "Vision",
                TechStack = "Stack"
            };
            _employerService.Setup(s => s.UpdateEmployer(It.IsAny<Employer>()))
                            .Throws(new InvalidOperationException("Employer not found."));

            // Act
            var result = page.OnPost();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(page.ModelState.ContainsKey(string.Empty));
            var error = Assert.Single(page.ModelState[string.Empty].Errors);
            Assert.Equal("An error occurred while updating the employer. Please try again.",
                         error.ErrorMessage);
        }

        #endregion
    }
}
