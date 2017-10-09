using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class EnumDefinitionProvider : DefinitionProviderBase<ObjectDefinitionBase, Type, Type>, IEnumDefinitionProvider<Type, Type>
    {
        public EnumDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase GetFromSource(Type parameter)
        {
            return new EnumDefinition
            {
                Name = parameter.GetReadableName(),
                FullName = parameter.GetReadableFullName()
            };
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase definition, IObjectDefinitions<Type> objectDefinitions, Type parameter)
        {
            var attributeProvider = this.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            var enumValueProvider = this.Resolve<IEnumValueDefinitionProvider<object, Type>>();

            var enumDefinition = definition as EnumDefinition;

            if (enumDefinition == null)
                return definition;

            enumDefinition.IsArray = false;

            parameter.GetTypeInfo()
                     .GetCustomAttributes()
                     .Select(x => attributeProvider.Process(attributeProvider.GetFromSource(x), objectDefinitions, x))
                     .ToList()
                     .ForEach(x => enumDefinition.AddAttribute(x));

            Enum.GetValues(parameter)
                .Cast<object>()
                .Select(x => enumValueProvider.Process(enumValueProvider.GetFromSource(x), objectDefinitions, x))
                .ToList()
                .ForEach(x => enumDefinition.AddValue(x));

            return enumDefinition;
        }
    }
}