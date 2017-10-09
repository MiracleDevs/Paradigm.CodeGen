using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public abstract class ObjectDefinitionProviderBase: DefinitionProviderBase<ObjectDefinitionBase, Type, Type>, IObjectDefinitionBaseProvider<Type, Type>
    {
        protected ObjectDefinitionProviderBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase definition, IObjectDefinitions<Type> objectDefinitions, Type parameter)
        {
            var typeInfo = parameter.GetTypeInfo();

            if (typeInfo.BaseType != null)
                definition.BaseType = objectDefinitions.Get(typeInfo.BaseType);

            typeInfo.ImplementedInterfaces?
                    .Select(objectDefinitions.Get)
                    .ToList()
                    .ForEach(definition.AddImplementedInterface);

            definition.Namespace = typeInfo.Namespace;
            definition.IsAbstract = typeInfo.IsAbstract;
            definition.IsInterface = typeInfo.IsInterface;

            if (typeInfo.IsArray && parameter != typeof(string))
            {
                definition.IsArray = true;
                definition.InnerObject = objectDefinitions.Get(parameter.GetElementType() ?? typeof(object));
            }
            else if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo) && parameter != typeof(string))
            {
                definition.IsArray = true;
                definition.InnerObject = objectDefinitions.Get((typeInfo.IsGenericType ? parameter.GenericTypeArguments.FirstOrDefault() : parameter.GetElementType()) ?? typeof(object));
            }

            if (typeInfo.IsGenericType)
            {
                
            }

            return definition;
        }
    }
}