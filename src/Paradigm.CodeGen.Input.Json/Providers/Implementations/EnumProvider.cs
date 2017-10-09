using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;
using Enum = Paradigm.CodeGen.Input.Json.Models.Enum;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class EnumProvider : ObjectBaseProviderBase, IEnumProvider
    {
        public EnumProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Create(ObjectBase input)
        {
            return new EnumDefinition
            {
                Name = input.Name,
                FullName = input.FullName
            };
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase output, ObjectBase input, IInputService service)
        {
            var enumValueProvider = this.Resolve<IEnumValueProvider>();
            var enumOutput = output as EnumDefinition;
            var enumInput = input as Enum;

            if (enumOutput == null || enumInput == null)
                return output;

            if (enumInput.Values != null)
                enumOutput.Values = enumInput.Values.Select(x => enumValueProvider.Process(enumValueProvider.Create(x), x, service)).ToList();

            return base.Process(output, input, service);
        }
    }
}