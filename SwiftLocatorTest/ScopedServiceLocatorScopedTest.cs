using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ScopedServiceLocatorScopedTest
    {
        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsServiceAfterRegisteringByGenericType()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register<TestScoped>();

            // Act
            var scopedService = ServiceLocator.GetScoped<TestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(scopedService);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsServiceAfterRegisteringByGenericInterfaceType()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register<ITestScoped, TestScoped>();

            // Act
            var scopedService = ServiceLocator.GetScoped<ITestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(scopedService);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsServiceAfterRegisteringWithFactory()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register(_ => new TestScoped());

            // Act
            var transientService = ServiceLocator.GetScoped<TestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetScopedt_ReturnsServiceAfterRegisteringWithFactoryByInterface()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register<ITestScoped, TestScoped>(_ => new TestScoped());

            // Act
            var transientService = ServiceLocator.GetScoped<ITestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsServiceAfterRegisteringWithInstance()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register(new TestScoped());

            // Act
            var transientService = ServiceLocator.GetScoped<TestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsServiceAfterRegisteringWithInstanceByInterface()
        {
            // Arrange
            const string scopeKey = "test scope key";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register<ITestScoped, TestScoped>(new TestScoped());

            // Act
            var transientService = ServiceLocator.GetScoped<ITestScoped>(scopeKey);

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsSameInstanceForSameScopeEveryTime()
        {
            // Arrange
            const string scopeKey = "test scope key";
            const string testString = "Test";
            ServiceLocator.RestartScopedScope(scopeKey);
            ServiceLocator.GetScopedRegistrator(scopeKey).Register<ITestScoped, TestScoped>();

            // Act
            var singletonServiceFirstInstance = ServiceLocator.GetScoped<ITestScoped>(scopeKey);
            singletonServiceFirstInstance.Name = testString;
            var singletonServiceSecondInstance = ServiceLocator.GetScoped<ITestScoped>(scopeKey);

            // Assert
            Assert.AreEqual(singletonServiceSecondInstance.Name, testString);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsDifferentInstanceForSameScopeEveryTime()
        {
            // Arrange
            const string scopeKey1 = "test scope key";
            const string scopeKey2 = "test scope key 2";
            const string testString = "Test";

            ServiceLocator.RestartScopedScope(scopeKey1);
            ServiceLocator.GetScopedRegistrator(scopeKey1).Register<ITestScoped, TestScoped>();

            ServiceLocator.RestartScopedScope(scopeKey2);
            ServiceLocator.GetScopedRegistrator(scopeKey2).Register<ITestScoped, TestScoped>();

            // Act
            var singletonServiceFirstInstance = ServiceLocator.GetScoped<ITestScoped>(scopeKey1);
            singletonServiceFirstInstance.Name = testString;
            var singletonServiceSecondInstance = ServiceLocator.GetScoped<ITestScoped>(scopeKey2);

            // Assert
            Assert.AreNotEqual(singletonServiceSecondInstance.Name, testString);
        }

        private interface ITestScoped
        {
            string Name { get; set; }
        }

        private class TestScoped : ITestScoped
        {
            public string Name { get; set; } = "default name";
        }
    }
}
