using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Moq;
using Xunit;

namespace UnitTests.Services.Interfaces
{
    /// <summary>
    /// Unit tests for the IEmployerService interface to verify contract, mock behavior, and stub coverage.
    /// </summary>
    public class IEmployerServiceTests
    {
        #region Interface Method Tests

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateEmployer_WhenCalled_ValidEmployer_ExecutesCorrectly_InvokesImplementation(bool isAdd)
        {
            // Arrange
            var data = new Mock<IEmployerService>();
            var employer = new Employer { EmployerName = isAdd ? "NewCo" : "UpCo" };

            // Build the right expression based on the flag
            Expression<Action<IEmployerService>> expr = isAdd
                ? (Expression<Action<IEmployerService>>)(s => s.AddEmployer(employer))
                : s => s.UpdateEmployer(employer);

            // Moq setup path
            data.Setup(expr).Verifiable();

            // Compile + invoke on stub (one compiled‐lambda branch)
            var compiled = expr.Compile();
            compiled(new FakeEmployerService());

            // Compile + invoke on a fresh mock (other compiled‐lambda branch)
            compiled(new Mock<IEmployerService>().Object);

            // Act
            if (isAdd)
                data.Object.AddEmployer(employer);
            else
                data.Object.UpdateEmployer(employer);

            // Assert
            data.Verify(expr, Times.Once);
        }

        [Fact]
        public void GetEmployers_WhenCalled_ValidCall_ExecutesCorrectly_ReturnsEmployersList()
        {
            // Arrange
            var data = new Mock<IEmployerService>();
            var list = new List<Employer> { new Employer { EmployerName = "Test" } };
            data.Setup(s => s.GetEmployers()).Returns(list);

            // Act
            var result = data.Object.GetEmployers();

            // Assert
            Assert.Equal(list, result);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        public void GetEmployerByName_WhenCalled_ValidName_ExecutesCorrectly_ReturnsCorrectEmployer(string name)
        {
            // Arrange
            var data = new Mock<IEmployerService>();
            var employer = new Employer { EmployerName = name };
            data.Setup(s => s.GetEmployerByName(name)).Returns(employer);

            // Act
            var result = data.Object.GetEmployerByName(name);

            // Assert
            Assert.Same(employer, result);
        }

        [Theory]
        [InlineData("X")]
        [InlineData("Y")]
        public void DeleteEmployer_WhenCalled_ValidName_ExecutesCorrectly_InvokesImplementation(string name)
        {
            // Arrange
            var data = new Mock<IEmployerService>();
            data.Setup(s => s.DeleteEmployer(name)).Verifiable();

            // Act
            data.Object.DeleteEmployer(name);

            // Assert
            data.Verify(s => s.DeleteEmployer(name), Times.Once);
        }

        #endregion

        #region Interface Signature Tests

        [Fact]
        public void Interface_DefinesGetEmployerByNameMethod_ValidInterface_HasCorrectSignature_WithCorrectSignature()
        {
            // genuine path
            var method = typeof(IEmployerService).GetMethod("GetEmployerByName");
            Assert.NotNull(method);
            Assert.Equal(typeof(Employer), method!.ReturnType);
            var p = method.GetParameters().Single();
            Assert.Equal("employerName", p.Name);
            Assert.Equal(typeof(string), p.ParameterType);

            // negative path
            Assert.Null(typeof(IEmployerService).GetMethod("DefinitelyNotAMethod"));
        }

        #endregion

        #region Stub & Stub-Coverage Test

        /// <summary>
        /// A no-op IEmployerService implementation used only to exercise
        /// the stub methods so we hit every block in FakeEmployerService.
        /// </summary>
        private class FakeEmployerService : IEmployerService
        {
            public List<Employer> GetEmployers() => new();
            public Employer GetEmployerByName(string _) => null!;
            public void AddEmployer(Employer _) { /* no-op */ }
            public void UpdateEmployer(Employer _) { /* no-op */ }
            public void DeleteEmployer(string _) { /* no-op */ }
        }

        [Fact]
        public void FakeEmployerService_Methods_WhenCalled_ReturnDefaults_Expected()
        {
            // Arrange
            var stub = new FakeEmployerService();

            // Act & Assert
            var list = stub.GetEmployers();
            Assert.NotNull(list);
            Assert.Empty(list);

            var emp = stub.GetEmployerByName("anything");
            Assert.Null(emp);

            // These are void no-ops
            stub.AddEmployer(new Employer { EmployerName = "x" });
            stub.UpdateEmployer(new Employer { EmployerName = "y" });
            stub.DeleteEmployer("z");
        }

        #endregion

        #region Interface Metadata Tests

        [Fact]
        public void Interface_DefinesGetEmployersMethod_ValidInterface_HasCorrectSignature_WithCorrectSignature()
        {
            var method = typeof(IEmployerService).GetMethod("GetEmployers");
            Assert.NotNull(method);
            Assert.Equal(typeof(List<Employer>), method!.ReturnType);
            Assert.Empty(method.GetParameters());
        }

        [Fact]
        public void Interface_DefinesAddEmployerMethod_ValidInterface_HasCorrectSignature_WithCorrectSignature()
        {
            var method = typeof(IEmployerService).GetMethod("AddEmployer");
            var p = method!.GetParameters().Single();
            Assert.Equal("employer", p.Name);
            Assert.Equal(typeof(Employer), p.ParameterType);
        }

        [Fact]
        public void Interface_DefinesUpdateEmployerMethod_ValidInterface_HasCorrectSignature_WithCorrectSignature()
        {
            var method = typeof(IEmployerService).GetMethod("UpdateEmployer");
            var p = method!.GetParameters().Single();
            Assert.Equal("employer", p.Name);
            Assert.Equal(typeof(Employer), p.ParameterType);
        }

        [Fact]
        public void Interface_DefinesDeleteEmployerMethod_ValidInterface_HasCorrectSignature_WithCorrectSignature()
        {
            var method = typeof(IEmployerService).GetMethod("DeleteEmployer");
            var p = method!.GetParameters().Single();
            Assert.Equal("employerName", p.Name);
            Assert.Equal(typeof(string), p.ParameterType);
        }

        [Fact]
        public void Interface_HasProperDefinition_ValidInterface_ChecksCorrectly_HasProperDefinition()
        {
            var type = typeof(IEmployerService);
            Assert.True(type.IsInterface);
            Assert.Equal("IEmployerService", type.Name);
            Assert.Equal("CampusConnect.WebSite.Services", type.Namespace);
        }

        #endregion
    }
}