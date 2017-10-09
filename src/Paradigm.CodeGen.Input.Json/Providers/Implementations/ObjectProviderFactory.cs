using System;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;
using Enum = Paradigm.CodeGen.Input.Json.Models.Enum;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public static class ObjectProviderFactory
    {
        public static IObjectBaseProvider<ObjectBase, ObjectDefinitionBase> GetProvider(IServiceProvider serviceProvider, ObjectBase objectBase)
        {
            if (objectBase is Class)
                return serviceProvider.GetService<IClassProvider>();

            if (objectBase is Enum)
                return serviceProvider.GetService<IEnumProvider>();

            if (objectBase is Struct)
                return serviceProvider.GetService<IStructProvider>();

            return null;
        }
    }
}