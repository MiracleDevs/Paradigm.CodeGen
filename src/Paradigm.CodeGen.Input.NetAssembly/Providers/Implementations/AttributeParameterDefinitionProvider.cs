using System;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class AttributeParameterDefinitionProvider: DefinitionProviderBase<AttributeParameterDefinition, Tuple<PropertyInfo, object>, Type>, IAttributeParameterDefinitionProvider<Tuple<PropertyInfo, object>, Type>
    {
        public AttributeParameterDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override AttributeParameterDefinition GetFromSource(Tuple<PropertyInfo, object> parameter)
        {
            return new AttributeParameterDefinition
            {
                Name = parameter.Item1.Name,
                Value = parameter.Item1.GetValue(parameter.Item2)?.ToString(),
                IsNumeric = parameter.Item1.PropertyType.IsNumeric()
            };
        }

        public override AttributeParameterDefinition Process(AttributeParameterDefinition definition, IObjectDefinitions<Type> objectDefinitions, Tuple<PropertyInfo, object> parameter)
        {
            return definition;
        }
    }
}