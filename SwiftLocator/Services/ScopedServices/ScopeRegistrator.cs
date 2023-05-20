using SwiftLocator.Services.DependencyInjectorServices;
using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public abstract class ScopeRegistrator : IServiceRegistrator
    {
        protected IDependencyInjector DependencyInjector;

#if NET5_0_OR_GREATER
        protected readonly Dictionary<Type, Func<object>> ServiceFactories = new();
        protected readonly Dictionary<Type, Type> RealTypes = new();
#else
        protected readonly Dictionary<Type, Func<object>> ServiceFactories = new Dictionary<Type, Func<object>>();
        protected readonly Dictionary<Type, Type> RealTypes = new Dictionary<Type, Type>();
#endif

        protected abstract IServiceProvider ServiceProvider { get; }

        public void SetDependencyInjector(IDependencyInjector dependencyInjector)
        {
            DependencyInjector = dependencyInjector;
        }

        public IServiceRegistrator Register<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => DependencyInjector.CreateInstanceWithDependencies<TInterface>());

            return this;
        }

        public IServiceRegistrator Register<T>() where T : class
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => DependencyInjector.CreateInstanceWithDependencies<T>());

            return this;
        }

        public IServiceRegistrator Register<T>(Func<IServiceProvider, T> factory)
            where T : class
        {
            ThrowTypeIsAlreadyRegistered<T>();

            ServiceFactories.Add(typeof(T), () => factory(ServiceProvider));

            return this;
        }

        public IServiceRegistrator Register<TInterface, TImplementation>(Func<IServiceProvider, TImplementation> factory) 
            where TImplementation : TInterface
        {
            ThrowTypeIsAlreadyRegistered<TInterface>();

            var type = typeof(TInterface);
            RealTypes.Add(type, typeof(TImplementation));
            ServiceFactories.Add(type, () => factory(ServiceProvider));

            return this;
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
