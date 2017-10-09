using System;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class ClassProvider : StructProviderBase, IClassProvider
    {
        public ClassProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Create(ObjectBase input)
        {
            return new ClassDefinition
            {
                Name = input.Name,
                FullName = input.FullName
            };
        }
    }
}