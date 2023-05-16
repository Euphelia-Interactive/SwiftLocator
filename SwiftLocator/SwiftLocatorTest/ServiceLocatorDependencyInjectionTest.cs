using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwiftLocator.Services.ServiceLocatorServices;

namespace SwiftLocatorTest
{
    [TestClass]
    public class ServiceLocatorDependencyInjectionTest
    {
        [TestMethod]
        public void CreateInstanceWithDependencies_ForTransientAndSingleton_InjectsDependencies()
        {
            ServiceLocator.Register(singletonRegistrator =>
            {
                singletonRegistrator.Register<ParentTestClass>();
            },
            transientRegistrator =>
            {
                transientRegistrator.Register<SecondChildInjectClass>();
                transientRegistrator.Register<ChildInjectClass>();
                transientRegistrator.Register<ChildChildInjectClass>();
            });
            var singletonService = ServiceLocator.GetSingleton<ParentTestClass>();

            Assert.IsNotNull(singletonService);
            Assert.IsNotNull(singletonService.InjectClass);
            Assert.IsNotNull(singletonService.InjectClass.ChildChildInjectClass);
            Assert.IsNotNull(singletonService.InjectClass.ChildChildInjectClass.TestString);
            Assert.IsNotNull(singletonService.SecondInjectClass);
            Assert.IsNotNull(singletonService.SecondInjectClass.TestString);
        }

        [TestMethod]
        public void CreateInstanceWithDependencies_ForScoped_InjectsDependencies()
        {
            const string scopeKey = "test";
            ScopedServiceLocator.Register(scopeKey, registrator =>
            {
                registrator.Register<ParentTestClass>();
                registrator.Register<SecondChildInjectClass>();
                registrator.Register<ChildInjectClass>();
                registrator.Register<ChildChildInjectClass>();
            });
            var scopedService = ScopedServiceLocator.Get<ParentTestClass>(scopeKey);

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
