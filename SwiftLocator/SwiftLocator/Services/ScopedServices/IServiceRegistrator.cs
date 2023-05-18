using System;

namespace SwiftLocator.Services.ScopedServices
{
    public interface IServiceRegistrator
    {
        IServiceRegistrator Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface;

        IServiceRegistrator Register<T>() 
            where T : class;

        IServiceRegistrator Register<T>(Func<IServiceProvider, T> factory)
            where T : class;

        IServiceRegistrator Register<TInterface, TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TInterface;
    }
}
