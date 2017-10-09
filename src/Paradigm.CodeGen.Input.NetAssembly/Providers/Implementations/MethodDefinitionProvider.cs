using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class MethodDefinitionProvider : DefinitionProviderBase<MethodDefinition, MethodInfo, Type>, IMethodDefinitionProvider<MethodInfo, Type>
    {
        public MethodDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override MethodDefinition GetFromSource(MethodInfo parameter)
        {
            return new MethodDefinition
            {
                Name = parameter.Name
            };
        }

        public override MethodDefinition Process(MethodDefinition definition, IObjectDefinitions<Type> objectDefinitions, MethodInfo parameter)
        {
            var attributeProvider = this.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            var parameterProvider = this.Resolve<IParameterDefinitionProvider<ParameterInfo, Type>>();
            var methodDefinition = definition;

            if (methodDefinition == null)
                return definition;

            methodDefinition.Name = parameter.Name;

            methodDefinition.ReturnType = objectDefinitions.Get(parameter.ReturnType);

            parameter.GetCustomAttributes()
                     .Select(x => attributeProvider.Process(attributeProvider.GetFromSource(x), objectDefinitions, x))
                     .ToList()
                     .ForEach(x => methodDefinition.AddAttribute(x));

            parameter.GetParameters()
                     .Select(x => parameterProvider.Process(parameterProvider.GetFromSource(x), objectDefinitions, x))
                     .ToList()
                     .ForEach(x => methodDefinition.AddParameter(x));           

            return definition;
        }
    }
}