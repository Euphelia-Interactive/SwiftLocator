using SwiftLocator.Models;
using SwiftLocator.Services.DependencyInjectorServices;
using SwiftLocator.Services.ScopedServices;
using System;

namespace SwiftLocator.Services.ServiceLocatorServices
{
    public static class ServiceLocator 
    {
        private static Scope _singletonScope;
        private static TransientScope _transientScope;

        public static void Register(Action<IScopedServiceRegistrator> registerSingletonServices = null,
            Action<IServiceRegistrator> registerTransientServices = null)
        {
            // Instantiate new scopes.
            _singletonScope = new Scope();
            _transientScope = new TransientScope();

            // Add dependency injector.
            var dependencyInjectorConfigurations =  new DependencyInjectorConfigurations(_singletonScope, _transientScope);
            var dependencyInjector = new DepedencyInjector(dependencyInjectorConfigurations);
            _singletonScope.SetDependencyInjector(dependencyInjector);
            _transientScope.SetDependencyInjector(dependencyInjector);

            // Register scopes.
            if(registerSingletonServices is not null)
                registerSingletonServices(_singletonScope);
            if(registerTransientServices is not null)
                registerTransientServices(_transientScope);

            // Test registration by building all instances.
            _singletonScope.BuildAllInstances();
            _transientScope.BuildAllInstances();
        }

        public static T GetSingleton<T>()
        {
            return _singletonScope.Get<T>();
        }

        public static T GetTransient<T>()
        {
            return _transientScope.Get<T>();
        }
    }
}
