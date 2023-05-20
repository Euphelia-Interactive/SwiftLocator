using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorSingletonTest
    {

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringByGenericType()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register<TestSingleton>();

            // Act
            var singletonService = ServiceLocator.GetSingleton<TestSingleton>();

            // Assert
            Assert.IsNotNull(singletonService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringByGenericInterfaceType()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register<ITestSinleton, TestSingleton>();

            // Act
            var singletonService = ServiceLocator.GetSingleton<ITestSinleton>();

            // Assert
            Assert.IsNotNull(singletonService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringWithFactory()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register(_ => new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<TestSingleton>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringWithFactoryByInterface()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register<ITestSinleton, TestSingleton>(_ => new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<ITestSinleton>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringWithInstance()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register(new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<TestSingleton>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsServiceAfterRegisteringWithInstanceByInterface()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register<ITestSinleton, TestSingleton>(new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<ITestSinleton>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsSameInstanceEveryTime()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            const string testString = "Test";
            ServiceLocator.SingletonRegistrator.Register<ITestSinleton, TestSingleton>();

            // Act
            var singletonServiceFirstInstance = ServiceLocator.GetSingleton<ITestSinleton>();
            singletonServiceFirstInstance.Name = testString;
            var singletonServiceSecondInstance = ServiceLocator.GetSingleton<ITestSinleton>();

            // Assert
            Assert.AreEqual(singletonServiceSecondInstance.Name, testString);
        }

        private interface ITestSinleton
        {
            string Name { get; set; }
        }

        private class TestSingleton : ITestSinleton
        {
            public string Name { get; set; } = "default name";
        }
    }
}
