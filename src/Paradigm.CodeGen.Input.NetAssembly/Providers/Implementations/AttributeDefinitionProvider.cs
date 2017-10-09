using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class AttributeDefinitionProvider : DefinitionProviderBase<AttributeDefinition, Attribute, Type>, IAttributeDefinitionProvider<Attribute, Type>
    {
        public AttributeDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override AttributeDefinition GetFromSource(Attribute parameter)
        {
            return new AttributeDefinition { Name = parameter.GetType().Name };
        }

        public override AttributeDefinition Process(AttributeDefinition definition, IObjectDefinitions<Type> objectDefinitions, Attribute parameter)
        {
            var attributeParameterProvider = this.Resolve<IAttributeParameterDefinitionProvider<Tuple<PropertyInfo, object>, Type>>();
            var type = parameter.GetType();

            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();

                foreach (var property in typeInfo.DeclaredProperties.Where(x => (x.GetMethod?.IsPublic ?? false) && (x.SetMethod?.IsPublic ?? false)))
                {
                    var tuple = new Tuple<PropertyInfo, object>(property, parameter);
                    definition.AddParameter(attributeParameterProvider.Process(attributeParameterProvider.GetFromSource(tuple), objectDefinitions, tuple));
                }

                type = typeInfo.BaseType;
            }


            return definition;
        }
    }
}