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
    /// Unit tests for the Delete page model for employers.
    /// </summary>
    public class DeleteModelTests
    {
        private readonly Mock<ILogger<DeleteModel>> _logger = new();
        private readonly Mock<IEmployerService> _employerService = new();

        // A reusable test employer instance
        private readonly Employer _testEmployer = new()
        {
            EmployerName = "Test Employer",
            CompanyVision = "Test Vision",
            TechStack = "Test Stack"
        };

        #region Setup

        /// <summary>
        /// Creates a DeleteModel page with TempData and mocks wired up.
        /// </summary>
        private DeleteModel CreatePage(out TempDataDictionary tempData)
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

            var page = new DeleteModel(_logger.Object, _employerService.Object)
            {
                PageContext = pageContext,
                TempData = tempData
            };
            return page;
        }

        #endregion

        #region OnGet Tests

        /// <summary>
        /// Tests that OnGet returns NotFound when the id is null or empty.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnGet_NullOrEmptyId_ReturnsNotFound_ForInvalidInput_Expected(string? id)
        {
            // Arrange
            var data = CreatePage(out _);

            // Act
            var result = data.OnGet(id, null, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests that OnGet returns NotFound when the service returns null.
        /// </summary>
        [Fact]
        public void OnGet_EmployerNotFound_ReturnsNotFound_WhenServiceReturnsNull_Expected()
        {
            // Arrange
            var data = CreatePage(out _);
            _employerService
                .Setup(s => s.GetEmployerByName("nonexistent"))
                .Returns((Employer?)null);

            // Act
            var result = data.OnGet("nonexistent", null, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests that OnGet returns PageResult and sets Employer_To_Delete when a valid id is provided.
        /// </summary>
        [Fact]
        public void OnGet_ValidId_ReturnsPageWithEmployer_WhenEmployerExists_Expected()
        {
            // Arrange
            var data = CreatePage(out _);
            _employerService
                .Setup(s => s.GetEmployerByName(_testEmployer.EmployerName))
                .Returns(_testEmployer);

            // Act
            var result = data.OnGet(_testEmployer.EmployerName, null, 1);

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal(_testEmployer, data.Employer_To_Delete);
        }

        #endregion

        #region OnPost Tests

        /// <summary>
        /// Tests that OnPost returns an error when Employer_To_Delete is null or has no name.
        /// </summary>
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "Some Vision")]
        public void OnPost_NullOrEmptyEmployerName_RedirectsToDashboardWithError_ForMissingInformation_Expected(
            string? employerName,
            string? companyVision)
        {
            // Arrange
            var data = CreatePage(out var tempData);
            data.EmployerSearchTerm = "searchTerm";
            data.CurrentPage = 3;

            if (employerName == null)
            {
                data.Employer_To_Delete = null!;
            }
            else
            {
                data.Employer_To_Delete = new Employer
                {
                    EmployerName = employerName,
                    CompanyVision = companyVision
                };
            }

            // Act
            var result = data.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowEmployers", redirect.PageHandler);
            Assert.NotNull(redirect.RouteValues);
            Assert.Equal("searchTerm", redirect.RouteValues["employerSearchTerm"]);
            Assert.Equal(3, redirect.RouteValues["currentPage"]);
            Assert.Equal(
                "Could not delete employer: Information was missing. Please try again.",
                tempData["Error"]);
        }

        /// <summary>
        /// Tests that OnPost correctly handles the success scenario when deleting an employer.
        /// </summary>
        [Fact]
        public void OnPost_DeleteEmployer_Success_RedirectsWithSuccessMessage_Expected()
        {
            // Arrange
            var data = CreatePage(out var tempData);
            data.Employer_To_Delete = _testEmployer;
            data.EmployerSearchTerm = "searchTerm";
            data.CurrentPage = 7;

            _employerService
                .Setup(s => s.DeleteEmployer(_testEmployer.EmployerName))
                .Verifiable();

            // Act
            var result = data.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowEmployers", redirect.PageHandler);
            Assert.NotNull(redirect.RouteValues);
            Assert.Equal("searchTerm", redirect.RouteValues["employerSearchTerm"]);
            Assert.Equal(7, redirect.RouteValues["currentPage"]);
            Assert.Equal(
                $"Employer '{_testEmployer.EmployerName}' deleted successfully.",
                tempData["Message"]);

            _employerService.Verify(
                s => s.DeleteEmployer(_testEmployer.EmployerName),
                Times.Once);
        }

        /// <summary>
        /// Tests that OnPost correctly handles the exception scenario when deleting an employer.
        /// </summary>
        [Fact]
        public void OnPost_DeleteEmployer_ServiceThrows_RedirectsWithErrorMessage_Expected()
        {
            // Arrange
            var data = CreatePage(out var tempData);
            data.Employer_To_Delete = _testEmployer;
            data.EmployerSearchTerm = "searchTerm";
            data.CurrentPage = 7;

            _employerService
                .Setup(s => s.DeleteEmployer(_testEmployer.EmployerName))
                .Throws(new InvalidOperationException("simulated failure"));

            // Act
            var result = data.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowEmployers", redirect.PageHandler);
            Assert.NotNull(redirect.RouteValues);
            Assert.Equal("searchTerm", redirect.RouteValues["employerSearchTerm"]);
            Assert.Equal(7, redirect.RouteValues["currentPage"]);
            Assert.Equal(
                $"An error occurred while deleting employer '{_testEmployer.EmployerName}'. Please try again.",
                tempData["Error"]);
        }

        /// <summary>
        /// Tests that OnPost returns an error when attempting to delete a non-existent employer.
        /// </summary>
        [Fact]
        public void OnPost_EmployerNotFound_RedirectsToDashboardWithError_WhenEmployerDoesNotExist_Expected()
        {
            // Arrange
            var data = CreatePage(out var tempData);
            data.Employer_To_Delete = new Employer
            {
                EmployerName = "Nonexistent Employer",
                CompanyVision = "Vision",
                TechStack = "Stack"
            };
            _employerService
                .Setup(s => s.DeleteEmployer("Nonexistent Employer"))
                .Throws(new InvalidOperationException("Employer not found."));

            // Act
            var result = data.OnPost();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Dashboard", redirect.PageName);
            Assert.Equal("ShowEmployers", redirect.PageHandler);
            Assert.NotNull(redirect.RouteValues);
            Assert.Equal(
                $"An error occurred while deleting employer 'Nonexistent Employer'. Please try again.",
                tempData["Error"]);
        }

        #endregion
    }
}