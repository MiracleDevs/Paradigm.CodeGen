using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class ParameterDefinitionProvider : DefinitionProviderBase<ParameterDefinition, ParameterInfo, Type>, IParameterDefinitionProvider<ParameterInfo, Type>
    {
        public ParameterDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ParameterDefinition GetFromSource(ParameterInfo parameter)
        {
            return new ParameterDefinition
            {
                Name = parameter.Name
            };
        }

        public override ParameterDefinition Process(ParameterDefinition definition, IObjectDefinitions<Type> objectDefinitions, ParameterInfo parameter)
        {
            var attributeProvider = this.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            var parameterDefinition = definition;

            if (parameterDefinition == null)
                return definition;

            parameterDefinition.Name = parameter.Name;

            parameterDefinition.Type = objectDefinitions.Get(parameter.ParameterType);

            parameter.GetCustomAttributes()
                     .Select(x => attributeProvider.Process(attributeProvider.GetFromSource(x), objectDefinitions, x))
                     .ToList()
                     .ForEach(x => parameterDefinition.AddAttribute(x));
            
            return definition;
        }
    }
}