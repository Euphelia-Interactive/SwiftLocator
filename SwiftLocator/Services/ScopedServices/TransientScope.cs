using System;
using System.Collections.Generic;

namespace SwiftLocator.Services.ScopedServices
{
    public class TransientScope : ScopeRegistrator, IServiceProvider
    {
        public new IReadOnlyDictionary<Type, Type> RealTypes => base.RealTypes;

        protected override IServiceProvider ServiceProvider => this;

        public object Get(Type type)
        {
            if (!ServiceFactories.TryGetValue(type, out var factory))
                throw new Exception("Trying to retrieve not registered service.");
            return factory.Invoke();
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
    }
}
