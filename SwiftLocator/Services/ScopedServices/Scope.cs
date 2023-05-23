#if NET5_0_OR_GREATER
// .NET 5.0 or higher code
#else
using SwiftLocator.Models;
#endif

using SwiftLocator.Services.DependencyInjectorServices;
using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public class Scope : ScopeRegistrator, IScopedServiceRegistrator, IServiceProvider, IServiceInstanceProvider
    {
#if NET5_0_OR_GREATER
        private readonly Dictionary<Type, object> _instances = new();
#else
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
#endif

        public Scope()
        {
#if NET5_0_OR_GREATER
            DependencyInjector = new DepedencyInjector(new(this));
#else
            DependencyInjector = new DepedencyInjector(new DependencyInjectorConfigurations(this));
#endif
        }

        public new IReadOnlyDictionary<Type, Type> RealTypes => base.RealTypes;

        protected override IServiceProvider ServiceProvider => this;

        public IScopedServiceRegistrator Register<T>(T instance)
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => instance);

            return this;
        }

        public IScopedServiceRegistrator Register<TInterface, TImplementation>(TImplementation instance) where TImplementation : TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            base.RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => instance);

            return this;
        }

        public object Get(Type type)
        {
            if (_instances.TryGetValue(type, out var instance))
                return instance;

            if (!ServiceFactories.TryGetValue(type, out var factory))
                throw new Exception("Trying to retrieve not registered service.");

            var service = factory.Invoke();
            _instances.Add(type, service);
            return service;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public bool TryGetInstance(Type type, out object instance)
        {
            return _instances.TryGetValue(type, out instance);
        }
    }
}
