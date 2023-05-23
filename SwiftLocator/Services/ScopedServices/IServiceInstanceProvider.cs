using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public interface IServiceInstanceProvider
    {
        bool TryGetInstance(Type type, out object instance);
        IReadOnlyDictionary<Type, Type> RealTypes { get; }
    }
}
