using SwiftLocator.Services.DependencyInjectorServices;
using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public abstract class ScopeRegistrator : IServiceRegistrator
    {
        protected IDependencyInjector DependencyInjector;
        protected readonly Dictionary<Type, Func<object>> ServiceFactories = new();
        protected readonly Dictionary<Type, Type> RealTypes = new();

        protected abstract IServiceProvider ServiceProvider { get; }

        public void SetDependencyInjector(IDependencyInjector dependencyInjector)
        {
            DependencyInjector = dependencyInjector;
        }

        public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => DependencyInjector.CreateInstanceWithDependencies<TInterface>());
        }

        public void Register<T>() where T : class
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => DependencyInjector.CreateInstanceWithDependencies<T>());
        }

        public void Register<T>(Func<IServiceProvider, T> factory)
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => factory(ServiceProvider));
        }

        public void Register<TInterface, TImplementation>(Func<IServiceProvider, TImplementation> factory) 
            where TImplementation : TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => factory(ServiceProvider));
        }

        public void BuildAllInstances()
        {
            foreach (var type in ServiceFactories.Keys)
                _ = ServiceProvider.Get(type);
        }

        protected void ThrowTypeIsAlreadyRegistered<T>()
        {
            if (ServiceFactories.ContainsKey(typeof(T)))
                throw new Exception("Service is already registered.");
        }
    }
}
