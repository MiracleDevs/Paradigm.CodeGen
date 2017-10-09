using System;
using System.Reflection;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Implementations;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly
{
    public class InputPlugin : IInputPlugin
    {
        public void RegisterPlugin(InputConfiguration configuration, IDependencyBuilder builder)
        {
            builder.Register<IAttributeDefinitionProvider<Attribute, Type>, AttributeDefinitionProvider>();
            builder.Register<IAttributeParameterDefinitionProvider<Tuple<PropertyInfo, object>, Type>, AttributeParameterDefinitionProvider>();
            builder.Register<IClassDefinitionProvider<Type, Type>, ClassDefinitionProvider>();
            builder.Register<IEnumDefinitionProvider<Type, Type>, EnumDefinitionProvider>();
            builder.Register<IEnumValueDefinitionProvider<object, Type>, EnumValueDefinitionProvider>();
            builder.Register<IMethodDefinitionProvider<MethodInfo, Type>, MethodDefinitionProvider>();
            builder.Register<IParameterDefinitionProvider<ParameterInfo, Type>, ParameterDefinitionProvider>();
            builder.Register<IPropertyDefinitionProvider<PropertyInfo, Type>, PropertyDefinitionProvider>();
            builder.Register<IStructDefinitionProvider<Type, Type>, StructDefinitionProvider>();
            builder.Register<IObjectDefinitions<Type>, ObjectDefinitions>();
            builder.RegisterScoped<IInputService, InputService>();
        }
    }
}