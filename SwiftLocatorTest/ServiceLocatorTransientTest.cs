using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorTransientTest
    {

        [TestMethod]
        public void ServiceLocator_GetTransient_ReturnsServiceAfterRegisteringByGenericType()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            ServiceLocator.TransientRegistrator.Register<TestTransient>();

            // Act
            var transientService = ServiceLocator.GetTransient<TestTransient>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetTransient_ReturnsServiceAfterRegisteringByGenericInterfaceType()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            ServiceLocator.TransientRegistrator.Register<ITestTransient, TestTransient>();

            // Act
            var transientService = ServiceLocator.GetTransient<ITestTransient>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetTransient_ReturnsServiceAfterRegisteringWithFactory()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            ServiceLocator.TransientRegistrator.Register(_ => new TestTransient());

            // Act
            var transientService = ServiceLocator.GetTransient<TestTransient>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetTransient_ReturnsServiceAfterRegisteringWithFactoryByInterface()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            ServiceLocator.TransientRegistrator.Register<ITestTransient, TestTransient>(_ => new TestTransient());

            // Act
            var transientService = ServiceLocator.GetTransient<ITestTransient>();

            // Assert
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void ServiceLocator_GetTransient_ReturnsNewInstanceEveryTime()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            const string testString = "Test";
            ServiceLocator.TransientRegistrator.Register<ITestTransient, TestTransient>();

            // Act
            var singletonServiceFirstInstance = ServiceLocator.GetTransient<ITestTransient>();
            singletonServiceFirstInstance.Name = testString;
            var singletonServiceSecondInstance = ServiceLocator.GetTransient<ITestTransient>();

            // Assert
            Assert.AreNotEqual(singletonServiceSecondInstance.Name, testString);
        }

        [TestMethod]
        public void ServiceLocator_GetTransient_DependencyInjectionReturnsSameInstanceEveryTime()
        {
            // Arrange
            ServiceLocator.RestartTransientScope();
            ServiceLocator.TransientRegistrator
                .Register<ITestTransient, TestTransient>()
                .Register<TaskTransientSameInstance>()
                .Register<TaskTransientScecondSameInstance>();

            // Act
            var instance = ServiceLocator.GetTransient<ITestTransient>();
            var secondInstance = ServiceLocator.GetTransient<TaskTransientSameInstance>().GetTestTransient();
            var thirdInstance = ServiceLocator.GetTransient<TaskTransientScecondSameInstance>().GetTestTransient();

            // Assert
            Assert.AreNotSame(instance, secondInstance);
            Assert.AreNotSame(instance, thirdInstance);
        }

        private interface ITestTransient
        {
            string Name { get; set; }
        }

        private class TestTransient : ITestTransient
        {
            public string Name { get; set; } = "default name";
        }

        private class TaskTransientSameInstance
        {
            private readonly ITestTransient _testTransient;

            public TaskTransientSameInstance(ITestTransient testTransient)
            {
                _testTransient = testTransient;
            }

            public ITestTransient GetTestTransient()
            {
                return _testTransient;
            }
        }

        private class TaskTransientScecondSameInstance
        {
            private readonly ITestTransient _testTransient;

            public TaskTransientScecondSameInstance(ITestTransient testTransient)
            {
                _testTransient = testTransient;
            }

            public ITestTransient GetTestTransient()
            {
                return _testTransient;
            }
        }
    }
}
