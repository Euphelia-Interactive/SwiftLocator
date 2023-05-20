namespace SwiftLocator.Services.DependencyInjectorServices
{
    public interface IDependencyInjector
    {
        T CreateInstanceWithDependencies<T>();
    }
}
