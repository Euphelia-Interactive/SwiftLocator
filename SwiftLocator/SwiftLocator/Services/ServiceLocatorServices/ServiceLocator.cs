using SwiftLocator.Models;
using SwiftLocator.Services.DependencyInjectorServices;
using SwiftLocator.Services.ScopedServices;
using System;
using System.Collections.Generic;
using IServiceProvider = SwiftLocator.Services.ScopedServices.IServiceProvider;

namespace SwiftLocator.Services.ServiceLocatorServices
{
    public static class ServiceLocator 
    {
        private static Scope _singletonScope;
        private static TransientScope _transientScope;
        private static DepedencyInjector _dependencyInjector;
        private static readonly Dictionary<string, Scope> _scopedScope;

        static ServiceLocator()
        {
            // Initialize scoped data.
#if NET5_0_OR_GREATER
            _scopedScope = new();
#else
            _scopedScope = new Dictionary<string, Scope>();
#endif
            Initialize();
        }

        public static IScopedServiceRegistrator SingletonRegistrator => _singletonScope;
        public static IServiceProvider SingletonProvider => _singletonScope;
        public static IServiceRegistrator TransientRegistrator => _transientScope;
        public static IServiceProvider TransientProvider => _transientScope;

        public static IServiceProvider GetScopedProvider(string scopeKey)
        {
            return GetAddScopedScope(scopeKey);
        }

        public static IScopedServiceRegistrator GetScopedRegistrator(string scopeKey)
        {
            return GetAddScopedScope(scopeKey);
        }

        public static void RegsterSingleton(Action<IScopedServiceRegistrator> singletonRegistrationAction)
        {
            singletonRegistrationAction(_singletonScope);
        }

        public static void RegsterTransient(Action<IServiceRegistrator> transientRegistrationAction)
        {
            transientRegistrationAction(_transientScope);
        }

        public static void RegisterScoped(string scopeKey, Action<IScopedServiceRegistrator> registrationAction)
        {
            // Instantiate new scope.
            var scope = GetAddScopedScope(scopeKey);

            // Register services.
            registrationAction(scope);
        }

        public static T GetSingleton<T>()
        {
            return _singletonScope.Get<T>();
        }

        public static T GetTransient<T>()
        {
            return _transientScope.Get<T>();
        }

        public static T GetScoped<T>(string scopeKey)
        {
            return GetScopedProvider(scopeKey).Get<T>();
        }

        /// <summary>
        ///     This method usage is optional. It makes sure all instances can be built correctly.
        /// </summary>
        public static void Build()
        {
            // Test registration by building all instances.
            _singletonScope.BuildAllInstances();
            _transientScope.BuildAllInstances();
        }

        /// <summary>
        ///     This method usage is optional. It makes sure all instances can be built correctly.
        /// </summary>
        public static void Build(string scopeKey)
        {
            var scope = GetAddScopedScope(scopeKey);
            // Test registration by building all service instances.
            scope.BuildAllInstances();
        }

        public static void RestartSingletonScope()
        {
            _singletonScope = new Scope();
            ReconfigureDependencyInjector();
            _singletonScope.SetDependencyInjector(_dependencyInjector);
        }

        public static void RestartTransientScope()
        {
            _transientScope = new TransientScope();
            ReconfigureDependencyInjector();
            _transientScope.SetDependencyInjector(_dependencyInjector);
        }

        public static void RestartScopedScope(string scopeKey)
        {
            var newScope = new Scope();
            _scopedScope[scopeKey] = newScope;
            var dependencyInjectorConfigurations = new DependencyInjectorConfigurations(newScope, _singletonScope, _transientScope);
            var depedencyInjector = new DepedencyInjector(dependencyInjectorConfigurations);
            newScope.SetDependencyInjector(depedencyInjector);
        }

        private static void ReconfigureDependencyInjector()
        {
            var dependencyInjectorConfigurations = new DependencyInjectorConfigurations(_singletonScope, _transientScope);
            _dependencyInjector.Reconfigure(dependencyInjectorConfigurations);
        }

        private static void Initialize()
        {
            // Instantiate new scopes.
            _singletonScope = new Scope();
            _transientScope = new TransientScope();

            // Setup dependency injector.
            var dependencyInjectorConfigurations = new DependencyInjectorConfigurations(_singletonScope, _transientScope);
            _dependencyInjector = new DepedencyInjector(dependencyInjectorConfigurations);
            _singletonScope.SetDependencyInjector(_dependencyInjector);
            _transientScope.SetDependencyInjector(_dependencyInjector);
        }

        private static Scope GetAddScopedScope(string scopeKey)
        {
            if (_scopedScope.TryGetValue(scopeKey, out var scope))
                return scope;

            var newScope = new Scope();
            _scopedScope[scopeKey] = newScope;

            var dependencyInjectorConfigurations = new DependencyInjectorConfigurations(newScope, _singletonScope, _transientScope);
            var depedencyInjector = new DepedencyInjector(dependencyInjectorConfigurations);
            newScope.SetDependencyInjector(depedencyInjector);

            return newScope;
        }
    }
}
