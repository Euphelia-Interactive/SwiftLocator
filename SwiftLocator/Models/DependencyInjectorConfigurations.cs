using SwiftLocator.Services.ScopedServices;
using System.Collections.Generic;

namespace SwiftLocator.Models
{
    public class DependencyInjectorConfigurations
    {
        public DependencyInjectorConfigurations(params IServiceInstanceProvider[] serviceProviders)
        {
            ServiceInstanceProviders = serviceProviders;
        }

        public IReadOnlyCollection<IServiceInstanceProvider> ServiceInstanceProviders { get; }
    }
}
