namespace SwiftLocator.Services.ScopedServices
{
    public interface IScopedServiceRegistrator : IServiceRegistrator
    {
        IScopedServiceRegistrator Register<T>(T instance);

        IScopedServiceRegistrator Register<TInterface, TImplementation>(TImplementation instance) 
            where TImplementation : TInterface;
    }
}
