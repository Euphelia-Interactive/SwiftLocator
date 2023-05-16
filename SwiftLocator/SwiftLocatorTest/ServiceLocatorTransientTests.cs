using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorTransientTests
    {

        [TestMethod]
        public void RegisterTransient_Registers()
        {
            ServiceLocator.Register(registerTransientServices: transientRegistrator =>
            {
                transientRegistrator.Register<TestTransient>();
            });
            var transientService = ServiceLocator.GetTransient<TestTransient>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterTransient_RegistersWithInterface()
        {
            ServiceLocator.Register(registerTransientServices: transientRegistrator =>
            {
                transientRegistrator.Register<ITestTransient, TestTransient>();
            });
            var transientService = ServiceLocator.GetTransient<ITestTransient>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterTransient_RegistersWithFactory()
        {
            ServiceLocator.Register(registerTransientServices: transientRegistrator =>
            {
                transientRegistrator.Register(_ => new TestTransient());
            });
            var transientService = ServiceLocator.GetTransient<TestTransient>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterTransient_RegistersWithFactoryInterface()
        {
            ServiceLocator.Register(registerTransientServices: transientRegistrator =>
            {
                transientRegistrator.Register<ITestTransient, TestTransient>(_ => new TestTransient());
            });
            var transientService = ServiceLocator.GetTransient<ITestTransient>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void GetTransientService_ReturnsNewInstance()
        {
            const string testString = "Test";

            ServiceLocator.Register(registerTransientServices: transientRegistrator =>
            {
                transientRegistrator.Register<ITestTransient, TestTransient>();
            });

            var singletonServiceFirstInstance = ServiceLocator.GetTransient<ITestTransient>();

            // Change first instance.
            singletonServiceFirstInstance.Name = testString;

            // Assert is same intance.
            var singletonServiceSecondInstance = ServiceLocator.GetTransient<ITestTransient>();
            Assert.AreNotEqual(singletonServiceSecondInstance.Name, testString);
        }

        private interface ITestTransient
        {
            public string Name { get; set; }
        }

        private class TestTransient : ITestTransient
        {
            public string Name { get; set; } = "default name";
        }
    }
}
