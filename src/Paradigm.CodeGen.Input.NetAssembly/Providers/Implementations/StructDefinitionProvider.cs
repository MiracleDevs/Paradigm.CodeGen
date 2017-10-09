using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class StructDefinitionProvider : ObjectDefinitionProviderBase, IStructDefinitionProvider<Type, Type>
    {
        public StructDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase GetFromSource(Type parameter)
        {
            return new StructDefinition
            {
                Name = parameter.GetReadableName(),
                FullName = parameter.GetReadableFullName()
            };
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase definition, IObjectDefinitions<Type> objectDefinitions, Type parameter)
        {
            base.Process(definition, objectDefinitions, parameter);

            var attributeProvider = this.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            var propertyProvider = this.Resolve<IPropertyDefinitionProvider<PropertyInfo, Type>>();
            var methodProvider = this.Resolve<IMethodDefinitionProvider<MethodInfo, Type>>();
            var typeInfo = parameter.GetTypeInfo();

            var structDefinition = definition as StructDefinition;

            if (structDefinition == null)
                return definition;

            typeInfo.GetCustomAttributes()
                    .Select(x => attributeProvider.Process(attributeProvider.GetFromSource(x), objectDefinitions, x))
                    .ToList()
                    .ForEach(x => structDefinition.AddAttribute(x));

            typeInfo.DeclaredProperties
                    .Where(x => (x.GetMethod?.IsPublic ?? false) || (x.SetMethod?.IsPublic ?? false))
                    .Select(x => propertyProvider.Process(propertyProvider.GetFromSource(x), objectDefinitions, x))
                    .ToList()
                    .ForEach(x => structDefinition.AddProperty(x));

            typeInfo.DeclaredMethods
                    .Where(x => !x.IsSpecialName && x.IsPublic)
                    .Select(x => methodProvider.Process(methodProvider.GetFromSource(x), objectDefinitions, x))
                    .ToList()
                    .ForEach(x => structDefinition.AddMethod(x));

            typeInfo.GenericTypeArguments
                .Select(objectDefinitions.Get)
                .ToList()
                .ForEach(x => structDefinition.AddGenericArgument(x));

            return structDefinition;
        }
    }
}