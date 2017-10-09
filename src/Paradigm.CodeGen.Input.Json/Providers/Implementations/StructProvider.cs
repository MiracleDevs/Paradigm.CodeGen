using System;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class StructProvider: StructProviderBase, IStructProvider
    {
        public StructProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Create(ObjectBase input)
        {
            return new StructDefinition
            {
                Name = input.Name,
                FullName = input.FullName
            };
        }
    }
}