using System;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class EnumValueProvider : DefinitionProviderBase<EnumValue, EnumValueDefinition>, IEnumValueProvider
    {
        public EnumValueProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override EnumValueDefinition Create(EnumValue input)
        {
            return new EnumValueDefinition
            {
                Name = input.Name,
                Value = input.Value
            };
        }
    }
}