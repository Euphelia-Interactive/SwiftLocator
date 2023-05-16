using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ScopedServiceLocatorScopedTests
    {
        [TestMethod]
        public void RegisterScoped_Registers()
        {
            const string scopeKey = "test scope key";
            // Registering.
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<TestScoped>();
            });

            // Retrieving.
            var scopedService = ScopedServiceLocator.Get<TestScoped>(scopeKey);
            Assert.IsNotNull(scopedService);
        }

        [TestMethod]
        public void RegisterScoped_RegistersWithInterface()
        {
            const string scopeKey = "test scope key";
            // Registering.
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<TestScoped>();
            });

            // Retrieving.
            var scopedService = ScopedServiceLocator.Get<TestScoped>(scopeKey);
            Assert.IsNotNull(scopedService);
        }

        [TestMethod]
        public void RegisterScoped_RegistersWithFactory()
        {
            const string scopeKey = "test scope key";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register(_ => new TestScoped());
            });
            var transientService = ScopedServiceLocator.Get<TestScoped>(scopeKey);
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterScoped_RegistersWithFactoryInterface()
        {
            const string scopeKey = "test scope key";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<ITestScoped, TestScoped>(_ => new TestScoped());
            });
            var transientService = ScopedServiceLocator.Get<ITestScoped>(scopeKey);
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterScoped_RegistersWithInstance()
        {
            const string scopeKey = "test scope key";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register(new TestScoped());
            });
            var transientService = ScopedServiceLocator.Get<TestScoped>(scopeKey);
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void RegisterScoped_RegistersWithInstanceInterface()
        {
            const string scopeKey = "test scope key";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<ITestScoped, TestScoped>(new TestScoped());
            });
            var transientService = ScopedServiceLocator.Get<ITestScoped>(scopeKey);
            Assert.IsNotNull(transientService);
        }

        [TestMethod]
        public void GetScopedService_ReturnsSameInstance()
        {
            const string scopeKey = "test scope key";

            const string testString = "Test";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<ITestScoped, TestScoped>();
            });

            var singletonServiceFirstInstance = ScopedServiceLocator.Get<ITestScoped>(scopeKey);

            // Change first instance.
            singletonServiceFirstInstance.Name = testString;

            // Assert is same intance.
            var singletonServiceSecondInstance = ScopedServiceLocator.Get<ITestScoped>(scopeKey);
            Assert.AreEqual(singletonServiceSecondInstance.Name, testString);
        }

        [TestMethod]
        public void GetScopedService_ForDifferentScopes_ReturnsDifferentIntances()
        {
            const string scopeKey1 = "test scope key";
            const string scopeKey2 = "test scope key 2";

            const string testString = "Test";

            ScopedServiceLocator.Register(scopeKey1, registrator =>
            {
                registrator.Register<ITestScoped, TestScoped>();
            });

            ScopedServiceLocator.Register(scopeKey2, registrator =>
            {
                registrator.Register<ITestScoped, TestScoped>();
            });

            var singletonServiceFirstInstance = ScopedServiceLocator.Get<ITestScoped>(scopeKey1);

            // Change first instance.
            singletonServiceFirstInstance.Name = testString;

            // Assert is same intance.
            var singletonServiceSecondInstance = ScopedServiceLocator.Get<ITestScoped>(scopeKey2);
            Assert.AreNotEqual(singletonServiceSecondInstance.Name, testString);
        }

        private interface ITestScoped
        {
            public string Name { get; set; }
        }

        private class TestScoped : ITestScoped
        {
            public string Name { get; set; } = "default name";
        }
    }
}
