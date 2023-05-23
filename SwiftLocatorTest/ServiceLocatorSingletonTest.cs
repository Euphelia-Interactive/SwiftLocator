using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;
using System;

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
            ServiceLocator.SingletonRegistrator.Register<ITestSingleton, TestSingleton>();

            // Act
            var singletonService = ServiceLocator.GetSingleton<ITestSingleton>();

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
            ServiceLocator.SingletonRegistrator.Register<ITestSingleton, TestSingleton>(_ => new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<ITestSingleton>();

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
            ServiceLocator.SingletonRegistrator.Register<ITestSingleton, TestSingleton>(new TestSingleton());

            // Act
            var transientService = ServiceLocator.GetSingleton<ITestSingleton>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsSameInstanceEveryTime()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            const string testString = "Test";
            ServiceLocator.SingletonRegistrator.Register<ITestSingleton, TestSingleton>();

            // Act
            var singletonServiceFirstInstance = ServiceLocator.GetSingleton<ITestSingleton>();
            singletonServiceFirstInstance.Name = testString;
            var singletonServiceSecondInstance = ServiceLocator.GetSingleton<ITestSingleton>();

            // Assert
            Assert.AreEqual(singletonServiceSecondInstance.Name, testString);
        }

        [TestMethod]
        public void ServiceLocator_GetSingleton_DependencyInjectionReturnsSameInstanceEveryTime()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator
                .Register<ITestSingleton, TestSingleton>()
                .Register<TaskSingletonSameInstance>()
                .Register<TaskSingletonScecondSameInstance>();

            // Act
            var instance = ServiceLocator.GetSingleton<ITestSingleton>();
            var secondInstance = ServiceLocator.GetSingleton<TaskSingletonSameInstance>().GetTestSingleton();
            var thirdInstance = ServiceLocator.GetSingleton<TaskSingletonScecondSameInstance>().GetTestSingleton();

            // Assert
            Assert.AreSame(instance, secondInstance);
            Assert.AreSame(instance, thirdInstance);
        }

        private interface ITestSingleton
        {
            string Name { get; set; }
        }

        private class TestSingleton : ITestSingleton
        {
            public string Name { get; set; } = "default name";
        }

        private class TaskSingletonSameInstance
        {
            private readonly ITestSingleton _testSingleton;

            public TaskSingletonSameInstance(ITestSingleton testSingleton)
            {
                _testSingleton = testSingleton;
            }

            public ITestSingleton GetTestSingleton()
            {
                return _testSingleton;
            }
        }

        private class TaskSingletonScecondSameInstance
        {
            private readonly ITestSingleton _testSingleton;

            public TaskSingletonScecondSameInstance(ITestSingleton testSingleton)
            {
                _testSingleton = testSingleton;
            }

            public ITestSingleton GetTestSingleton()
            {
                return _testSingleton;
            }
        }
    }
}
