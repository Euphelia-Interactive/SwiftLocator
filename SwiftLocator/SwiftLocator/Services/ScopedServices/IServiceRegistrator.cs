using System;

namespace SwiftLocator.Services.ScopedServices
{
    public interface IServiceRegistrator
    {
        void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface;

        void Register<T>() where T : class;

        void Register<T>(Func<IServiceProvider, T> factory);

        void Register<TInterface, TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TInterface;
    }
}
