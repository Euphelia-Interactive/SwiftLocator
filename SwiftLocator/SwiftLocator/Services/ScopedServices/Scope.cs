using SwiftLocator.Services.DependencyInjectorServices;
using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public class Scope : ScopeRegistrator, IScopedServiceRegistrator, IServiceProvider
    {
        private readonly Dictionary<Type, object> _instances = new();

        public Scope()
        {
            DependencyInjector = new DepedencyInjector(new(this));
        }

        public new IReadOnlyDictionary<Type, Type> RealTypes => base.RealTypes;

        protected override IServiceProvider ServiceProvider => this;

        public void Register<T>(T instance)
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => instance);
        }

        public void Register<TInterface, TImplementation>(TImplementation instance) where TImplementation : TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            base.RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => instance);
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
    }
}
