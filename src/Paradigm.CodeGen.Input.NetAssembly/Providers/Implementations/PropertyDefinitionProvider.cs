using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class PropertyDefinitionProvider : DefinitionProviderBase<PropertyDefinition, PropertyInfo, Type>, IPropertyDefinitionProvider<PropertyInfo, Type>
    {
        public PropertyDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override PropertyDefinition GetFromSource(PropertyInfo parameter)
        {
            return new PropertyDefinition
            {
                Name = parameter.Name
            };
        }

        public override PropertyDefinition Process(PropertyDefinition definition, IObjectDefinitions<Type> objectDefinitions, PropertyInfo parameter)
        {
            var attributeProvider = this.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            var propertyDefinition = definition;

            if (propertyDefinition == null)
                return definition;

            propertyDefinition.Name = parameter.Name;

            propertyDefinition.Type = objectDefinitions.Get(parameter.PropertyType);

            parameter.GetCustomAttributes()
                     .Select(x => attributeProvider.Process(attributeProvider.GetFromSource(x), objectDefinitions, x))
                     .ToList()
                     .ForEach(x => propertyDefinition.AddAttribute(x));

            return definition;           
        }
    }
}