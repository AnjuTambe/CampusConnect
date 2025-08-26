using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace UnitTests.Services
{
    /// <summary>
    /// Exhaustive unit tests for <see cref="JsonFileEmployerService"/>.
    /// Achieves 100 % line & branch coverage while following project standards.
    /// </summary>
    public sealed class JsonFileEmployerServiceTests : IDisposable
    {
        private readonly string _tempRoot;
        private readonly Mock<IWebHostEnvironment> _envMock;

        // ────────────────────────────────────────────────────────────────
        //  Setup / Teardown
        // ────────────────────────────────────────────────────────────────
        public JsonFileEmployerServiceTests()
        {
            _tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Path.Combine(_tempRoot, "data"));

            _envMock = new Mock<IWebHostEnvironment>();
            _envMock.Setup(e => e.WebRootPath).Returns(_tempRoot);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, true);
        }

        // ────────────────────────────────────────────────────────────────
        //  GET EMPLOYERS
        // ────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "GetEmployers_NoFilters_ValidJson_Should_Return_All")]
        public void GetEmployers_NoFilters_ValidJson_Should_Return_All()
        {
            // Arrange
            var data = CreateSampleFile(3);
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Equal(data.Count, result.Count);
        }

        [Fact(DisplayName = "GetEmployers_PathResolutionError_Should_Return_Empty")]
        public void GetEmployers_PathResolutionError_Should_Return_Empty()
        {
            // Arrange
            _envMock.Setup(e => e.WebRootPath).Returns("bad\0path");
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "GetEmployers_FileOpenArgumentException_Should_Return_Empty")]
        public void GetEmployers_FileOpenArgumentException_Should_Return_Empty()
        {
            // Arrange – asterisk valid for directory, illegal for file
            _envMock.Setup(e => e.WebRootPath).Returns(Path.Combine(_tempRoot, "star*path"));
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "GetEmployers_FileMissing_Should_Return_Empty")]
        public void GetEmployers_FileMissing_Should_Return_Empty()
        {
            var service = new JsonFileEmployerService(_envMock.Object);

            var result = service.GetEmployers();

            Assert.Empty(result);
        }

        [Fact(DisplayName = "GetEmployers_InvalidJson_Should_Return_Empty")]
        public void GetEmployers_InvalidJson_Should_Return_Empty()
        {
            File.WriteAllText(GetJsonPath(), "☠ not json ☠");
            var service = new JsonFileEmployerService(_envMock.Object);

            var result = service.GetEmployers();

            Assert.Empty(result);
        }

        /// <summary>
        /// Simulates a sharing-violation <see cref="IOException"/> by locking the file
        /// before the service attempts to read it.
        /// </summary>
        [Fact(DisplayName = "GetEmployers_IOException_Should_Return_Empty")]
        public void GetEmployers_IOException_Should_Return_Empty()
        {
            // Arrange ★
            File.WriteAllText(GetJsonPath(), "[]");                       // create a valid file
            using var lockStream = new FileStream(                        // lock it exclusively
                GetJsonPath(), FileMode.Open, FileAccess.Read, FileShare.None);

            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
            // (lockStream disposed automatically → file unlocked)
        }

        [Fact(DisplayName = "GetEmployers_JsonDeserializesToNull_Should_Return_Empty")]
        public void GetEmployers_JsonDeserializesToNull_Should_Return_Empty()
        {
            // Arrange - Write "null" as JSON content which deserializes to null
            File.WriteAllText(GetJsonPath(), "null");
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "GetEmployers_FileNotFoundExceptionSpecific_Should_Return_Empty")]
        public void GetEmployers_FileNotFoundExceptionSpecific_Should_Return_Empty()
        {
            // Arrange - Use a path that will definitely not exist
            _envMock.Setup(e => e.WebRootPath).Returns(Path.Combine(_tempRoot, "nonexistent"));
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "GetEmployers_JsonExceptionSpecific_Should_Return_Empty")]
        public void GetEmployers_JsonExceptionSpecific_Should_Return_Empty()
        {
            // Arrange - Write malformed JSON that will cause JsonException
            File.WriteAllText(GetJsonPath(), "{\"incomplete\": json");
            var service = new JsonFileEmployerService(_envMock.Object);

            // Act
            var result = service.GetEmployers();

            // Assert
            Assert.Empty(result);
        }

        // ────────────────────────────────────────────────────────────────
        //  GET BY NAME
        // ────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "GetEmployerByName_ValidCaseInsensitive_Should_Return_Item")]
        public void GetEmployerByName_ValidCaseInsensitive_Should_Return_Item()
        {
            var data = CreateSampleFile(2);
            var service = new JsonFileEmployerService(_envMock.Object);

            var result = service.GetEmployerByName("fabrIKam");

            Assert.NotNull(result);
            Assert.Equal(data[0].EmployerName, result!.EmployerName);
        }

        [Theory(DisplayName = "GetEmployerByName_NullOrEmpty_Should_Return_Null")]
        [InlineData(null!)]
        [InlineData("")]
        public void GetEmployerByName_NullOrEmpty_Should_Return_Null(string name)
        {
            var service = new JsonFileEmployerService(_envMock.Object);

            var result = service.GetEmployerByName(name);

            Assert.Null(result);
        }

        // ────────────────────────────────────────────────────────────────
        //  ADD EMPLOYER
        // ────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "AddEmployer_Valid_Should_Persist")]
        public void AddEmployer_Valid_Should_Persist()
        {
            _ = CreateSampleFile(1);
            var service = new JsonFileEmployerService(_envMock.Object);

            var data = new Employer
            {
                EmployerName = "NewCo",
                CompanyVision = "Reach new heights",
                TechStack = "Azure, .NET"
            };

            service.AddEmployer(data);

            Assert.Contains(service.GetEmployers(), e => e.EmployerName == "NewCo");
        }

        [Fact(DisplayName = "AddEmployer_Null_Should_Throw_ArgumentNullException")]
        public void AddEmployer_Null_Should_Throw_ArgumentNullException()
        {
            var service = new JsonFileEmployerService(_envMock.Object);

            Assert.Throws<ArgumentNullException>(() => service.AddEmployer(null!));
        }

        [Fact(DisplayName = "AddEmployer_EmptyName_Should_Throw_ArgumentException")]
        public void AddEmployer_EmptyName_Should_Throw_ArgumentException()
        {
            var service = new JsonFileEmployerService(_envMock.Object);
            var data = new Employer { EmployerName = string.Empty };

            Assert.Throws<ArgumentException>(() => service.AddEmployer(data));
        }

        [Fact(DisplayName = "AddEmployer_DuplicateName_Should_Throw_InvalidOperationException")]
        public void AddEmployer_DuplicateName_Should_Throw_InvalidOperationException()
        {
            _ = CreateSampleFile(1);                                      // includes Fabrikam
            var service = new JsonFileEmployerService(_envMock.Object);
            var data = new Employer { EmployerName = "Fabrikam" };

            Assert.Throws<InvalidOperationException>(() => service.AddEmployer(data));
        }

        [Fact(DisplayName = "AddEmployer_SaveIOException_Should_Throw_IOException")]
        public void AddEmployer_SaveIOException_Should_Throw_IOException()
        {
            _envMock.Setup(e => e.WebRootPath).Returns("bad\0root");      // invalid path → File.Create throws
            var service = new JsonFileEmployerService(_envMock.Object);
            var data = new Employer { EmployerName = "FailCo" };

            Assert.Throws<IOException>(() => service.AddEmployer(data));
        }

        // ────────────────────────────────────────────────────────────────
        //  UPDATE EMPLOYER
        // ────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "UpdateEmployer_Valid_Should_Persist_Changes")]
        public void UpdateEmployer_Valid_Should_Persist_Changes()
        {
            // Arrange
            var data = CreateSampleFile(1)[0];
            var service = new JsonFileEmployerService(_envMock.Object);
            data.CompanyVision = "Innovating the future";

            // Act
            service.UpdateEmployer(data);

            // Assert
            var result = service.GetEmployerByName("Fabrikam");
            Assert.NotNull(result);                                   // ensures non-null
            Assert.Equal("Innovating the future", result!.CompanyVision);
        }

        [Fact(DisplayName = "UpdateEmployer_Null_Should_Throw_ArgumentNullException")]
        public void UpdateEmployer_Null_Should_Throw_ArgumentNullException()
        {
            var service = new JsonFileEmployerService(_envMock.Object);

            Assert.Throws<ArgumentNullException>(() => service.UpdateEmployer(null!));
        }

        [Fact(DisplayName = "UpdateEmployer_EmptyName_Should_Throw_ArgumentException")]
        public void UpdateEmployer_EmptyName_Should_Throw_ArgumentException()
        {
            var service = new JsonFileEmployerService(_envMock.Object);
            var data = new Employer { EmployerName = string.Empty };

            Assert.Throws<ArgumentException>(() => service.UpdateEmployer(data));
        }

        [Fact(DisplayName = "UpdateEmployer_NotFound_Should_Throw_InvalidOperationException")]
        public void UpdateEmployer_NotFound_Should_Throw_InvalidOperationException()
        {
            _ = CreateSampleFile(1);
            var service = new JsonFileEmployerService(_envMock.Object);
            var data = new Employer { EmployerName = "GhostCo" };

            Assert.Throws<InvalidOperationException>(() => service.UpdateEmployer(data));
        }

        // ────────────────────────────────────────────────────────────────
        //  DELETE EMPLOYER
        // ────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "DeleteEmployer_Existing_Should_Remove")]
        public void DeleteEmployer_Existing_Should_Remove()
        {
            _ = CreateSampleFile(1);
            var service = new JsonFileEmployerService(_envMock.Object);

            service.DeleteEmployer("Fabrikam");

            Assert.DoesNotContain(service.GetEmployers(), e => e.EmployerName == "Fabrikam");
        }

        [Theory(DisplayName = "DeleteEmployer_NullOrEmpty_Should_Throw_ArgumentException")]
        [InlineData(null!)]
        [InlineData("")]
        public void DeleteEmployer_NullOrEmpty_Should_Throw_ArgumentException(string name)
        {
            var service = new JsonFileEmployerService(_envMock.Object);

            Assert.Throws<ArgumentException>(() => service.DeleteEmployer(name));
        }

        [Fact(DisplayName = "DeleteEmployer_NotFound_Should_Throw_InvalidOperationException")]
        public void DeleteEmployer_NotFound_Should_Throw_InvalidOperationException()
        {
            _ = CreateSampleFile(1);
            var service = new JsonFileEmployerService(_envMock.Object);

            Assert.Throws<InvalidOperationException>(() => service.DeleteEmployer("GhostCo"));
        }

        // ────────────────────────────────────────────────────────────────
        //  Helper Methods
        // ────────────────────────────────────────────────────────────────
        private List<Employer> CreateSampleFile(int count)
        {
            var list = Enumerable.Range(0, count).Select(i => new Employer
            {
                EmployerName = i switch { 0 => "Fabrikam", 1 => "Contoso", _ => "ACME" },
                CompanyVision = $"Vision {i}",
                TechStack = $"Stack {i}"
            }).ToList();

            File.WriteAllText(
                GetJsonPath(),
                JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true })
            );

            return list;
        }

        private string GetJsonPath() =>
            Path.Combine(_tempRoot, "data", "employer.json");
    }
}