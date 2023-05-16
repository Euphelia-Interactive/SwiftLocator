using SwiftLocator.Services.ScopedServices;
using System;
using System.Collections.Generic;
using IServiceProvider = SwiftLocator.Services.ScopedServices.IServiceProvider;

namespace SwiftLocator.Services.ServiceLocatorServices
{
    public class ScopedServiceLocator
    {
        private static readonly Dictionary<string, Scope> _scopedScope = new();

        public static void Register(string scopeKey, Action<IScopedServiceRegistrator> registrationAction)
        {
            // Instantiate new scope.
            var newScope = new Scope();
            _scopedScope[scopeKey] = newScope;

            // Register services.
            registrationAction(newScope);

            // Test registration by building all service instances.
            newScope.BuildAllInstances();
        }

        public static T Get<T>(string scopeKey)
        {
            return GetServiceProvider(scopeKey).Get<T>();
        }

        public static IServiceProvider GetServiceProvider(string scopeKey)
        {
            if (!_scopedScope.TryGetValue(scopeKey, out var scope))
                throw new Exception("Trying to get not registered service.");
            return scope;
        }
    }
}
