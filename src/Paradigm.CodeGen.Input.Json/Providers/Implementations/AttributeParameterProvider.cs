using System;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class AttributeParameterProvider: DefinitionProviderBase<AttributeParameter, AttributeParameterDefinition>, IAttributeParameterProvider
    {
        public AttributeParameterProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override AttributeParameterDefinition Create(AttributeParameter input)
        {
            return new AttributeParameterDefinition
            {
                Name = input.Name,
                Value = input.Value,
                IsNumeric = input.IsNumeric
            };
        }
    }
}