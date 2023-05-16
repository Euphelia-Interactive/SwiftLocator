using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorSingletonTests
    {
        [TestMethod]
        public void RegisterSingleton_Registers()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<TestSingleton>();
            });
            var singletonService = ServiceLocator.GetSingleton<TestSingleton>();
            Assert.IsNotNull(singletonService);
        }

        [TestMethod]
        public void RegisterSingleton_RegistersWithInterface()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<ITestSinleton, TestSingleton>();
            });
            var singletonService = ServiceLocator.GetSingleton<ITestSinleton>();
            Assert.IsNotNull(singletonService);
        }

        [TestMethod]
        public void RegisterSingleton_RegistersWithFactory()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register(_ => new TestSingleton());
            });
            var transientService = ServiceLocator.GetSingleton<TestSingleton>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterSingleton_RegistersWithFactoryInterface()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<ITestSinleton, TestSingleton>(_ => new TestSingleton());
            });
            var transientService = ServiceLocator.GetSingleton<ITestSinleton>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterSingleton_RegistersWithInstance()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register(new TestSingleton());
            });
            var transientService = ServiceLocator.GetSingleton<TestSingleton>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterSingleton_RegistersWithInstanceInterface()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<ITestSinleton, TestSingleton>(new TestSingleton());
            });
            var transientService = ServiceLocator.GetSingleton<ITestSinleton>();
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void GetSingletonService_ReturnsSameInstance()
        {
            const string testString = "Test";
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<ITestSinleton, TestSingleton>();
            });

            var singletonServiceFirstInstance = ServiceLocator.GetSingleton<ITestSinleton>();

            // Change first instance.
            singletonServiceFirstInstance.Name = testString;

            // Assert is same intance.
            var singletonServiceSecondInstance = ServiceLocator.GetSingleton<ITestSinleton>();
            Assert.AreEqual(singletonServiceSecondInstance.Name, testString);
        }

        private interface ITestSinleton
        {
            public string Name { get; set; }
        }

        private class TestSingleton : ITestSinleton
        {
            public string Name { get; set; } = "default name";
        }
    }
}
