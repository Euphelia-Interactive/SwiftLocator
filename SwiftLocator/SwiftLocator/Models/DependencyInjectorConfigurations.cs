using SwiftLocator.Services.ScopedServices;
using System.Collections.Generic;

namespace SwiftLocator.Models
{
    public class DependencyInjectorConfigurations
    {
        public DependencyInjectorConfigurations(params IServiceProvider[] serviceProviders)
        {
            ServiceProviders = serviceProviders;
        }

        public IReadOnlyCollection<IServiceProvider> ServiceProviders { get; }
    }
}
