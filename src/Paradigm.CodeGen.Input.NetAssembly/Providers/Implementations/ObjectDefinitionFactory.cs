using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public static class ObjectDefinitionFactory
    {
        public static IObjectDefinitionBaseProvider<Type, Type> GetProvider(IServiceProvider provider, Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsEnum)
                return provider.GetService<IEnumDefinitionProvider<Type, Type>>();

            if (typeInfo.IsValueType)
                return provider.GetService<IStructDefinitionProvider<Type, Type>>();

            return provider.GetService<IClassDefinitionProvider<Type, Type>>();
        }
    }
}