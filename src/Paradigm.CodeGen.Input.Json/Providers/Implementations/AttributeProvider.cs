using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;
using Attribute = Paradigm.CodeGen.Input.Json.Models.Attribute;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class AttributeProvider : DefinitionProviderBase<Attribute, AttributeDefinition>, IAttributeProvider
    {
        public AttributeProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override AttributeDefinition Create(Attribute input)
        {
            return new AttributeDefinition
            {
                Name = input.Name            
            };
        }

        public override AttributeDefinition Process(AttributeDefinition output, Attribute input, IInputService service)
        {
            var attributeParameterProvider = this.Resolve<IAttributeParameterProvider>();
            output.Parameters = input.Parameters.Select(x => attributeParameterProvider.Process(attributeParameterProvider.Create(x), x, service)).ToList();
            return base.Process(output, input, service);
        }
    }
}