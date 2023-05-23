﻿using SwiftLocator.Models;
using System;
using System.Linq;

namespace SwiftLocator.Services.DependencyInjectorServices
{
    public class DepedencyInjector : IDependencyInjector
    {
        private DependencyInjectorConfigurations _configurations;
        
        public DepedencyInjector(DependencyInjectorConfigurations configurations)
        {
            _configurations = configurations;
        }

        public void Reconfigure(DependencyInjectorConfigurations configurations)
        {
            _configurations = configurations;
        }

        public T CreateInstanceWithDependencies<T>()
        {
            return (T)CreateInstanceWithDependencies(typeof(T));
        }

        private object CreateInstanceWithDependencies(Type representativeType)
        {
            var realType = GetRealType(representativeType);

            var constructor = realType.GetConstructors().FirstOrDefault();
            if (constructor is null)
                return Activator.CreateInstance(realType);

            var parameters = constructor.GetParameters();

            var resolvedParameters = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                if (TryGetInstance(parameterType, out var instance))
                    resolvedParameters[i] = instance;
                else
                    resolvedParameters[i] = CreateInstanceWithDependencies(parameterType);
            }

            return Activator.CreateInstance(realType, resolvedParameters);
        }

        private Type GetRealType(Type representativeType)
        {
            // Try get type from real types otherwise use same type.
            foreach (var serviceProvider in _configurations.ServiceInstanceProviders)
                if (serviceProvider.RealTypes.TryGetValue(representativeType, out var realType))
                    return realType;
            return representativeType;
        }

        private bool TryGetInstance(Type type, out object instance)
        {
            foreach(var instanceProvider in _configurations.ServiceInstanceProviders)
                if(instanceProvider.TryGetInstance(type, out instance))
                    return true;
            instance = null;
            return false;
        }
    }
}
