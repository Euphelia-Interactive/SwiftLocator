using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public interface IServiceProvider
    {
        T Get<T>();
        object Get(Type type);
        IReadOnlyDictionary<Type, Type> RealTypes { get; }
    }
}
