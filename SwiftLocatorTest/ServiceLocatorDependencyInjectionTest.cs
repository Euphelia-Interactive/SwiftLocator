using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorDependencyInjectionTest
    {
        [TestMethod]
        public void ServiceLocator_GetSingleton_ReturnsInstanceWithInjectedDependencies()
        {
            // Arrange
            ServiceLocator.RestartSingletonScope();
            ServiceLocator.SingletonRegistrator.Register<ParentTestClass>();

            ServiceLocator.TransientRegistrator
                .Register<SecondChildInjectClass>()
                .Register<ChildInjectClass>()
                .Register<ChildChildInjectClass>();

            // Act
            var singletonService = ServiceLocator.GetSingleton<ParentTestClass>();

            // Assert
            Assert.IsNotNull(singletonService);
            Assert.IsNotNull(singletonService.InjectClass);
            Assert.IsNotNull(singletonService.InjectClass.ChildChildInjectClass);
            Assert.IsNotNull(singletonService.InjectClass.ChildChildInjectClass.TestString);
            Assert.IsNotNull(singletonService.SecondInjectClass);
            Assert.IsNotNull(singletonService.SecondInjectClass.TestString);
        }

        [TestMethod]
        public void ServiceLocator_GetScoped_ReturnsInstanceWithInjectedDependencies()
        {
            // Arrange
            const string scopeKey = "test";

            ServiceLocator.GetScopedRegistrator(scopeKey)
                .Register<ParentTestClass>()
                .Register<SecondChildInjectClass>()
                .Register<ChildInjectClass>()
                .Register<ChildChildInjectClass>();

            // Act
            var scopedService = ServiceLocator.GetScoped<ParentTestClass>(scopeKey);

            // Assert
            Assert.IsNotNull(scopedService);
            Assert.IsNotNull(scopedService.InjectClass);
            Assert.IsNotNull(scopedService.InjectClass.ChildChildInjectClass);
            Assert.IsNotNull(scopedService.InjectClass.ChildChildInjectClass.TestString);
            Assert.IsNotNull(scopedService.SecondInjectClass);
            Assert.IsNotNull(scopedService.SecondInjectClass.TestString);
        }

        public class ParentTestClass
        {
            public ChildInjectClass InjectClass { get; }
            public SecondChildInjectClass SecondInjectClass { get; }

            public ParentTestClass(ChildInjectClass injectClass, SecondChildInjectClass secondInjectClass)
            {
                InjectClass = injectClass;
                SecondInjectClass = secondInjectClass;
            }
        }

        public class SecondChildInjectClass
        {
            public string TestString { get; set; } = "random text 2";
        }

        public class ChildInjectClass
        {
            public ChildChildInjectClass ChildChildInjectClass { get; }

            public ChildInjectClass(ChildChildInjectClass childChildInjectClass)
            {
                ChildChildInjectClass = childChildInjectClass;
            }
        }

        public class ChildChildInjectClass
        {
            public string TestString { get; set; } = "random text";
        }
    }
}
