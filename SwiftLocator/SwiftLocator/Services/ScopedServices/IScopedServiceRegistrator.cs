namespace SwiftLocator.Services.ScopedServices
{
    public interface IScopedServiceRegistrator : IServiceRegistrator
    {
        void Register<T>(T instance);

        void Register<TInterface, TImplementation>(TImplementation instance) 
            where TImplementation : TInterface;
    }
}
