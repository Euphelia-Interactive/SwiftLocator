using System;

namespace SwiftLocator.Services.ScopedServices
{
    public interface IServiceProvider
    {
        T Get<T>();
        object Get(Type type);
    }
}
