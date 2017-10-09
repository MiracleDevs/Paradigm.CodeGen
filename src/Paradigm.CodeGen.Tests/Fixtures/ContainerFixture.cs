using System;
using System.Reflection;
using Paradigm.CodeGen.Input;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Implementations;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.CodeGen.Tests.Mocks;
using Paradigm.Core.DependencyInjection;
using Paradigm.Core.DependencyInjection.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly;
using Paradigm.CodeGen.Logging;

namespace Paradigm.CodeGen.Tests.Fixtures
{
    public class ContainerFixture : IDisposable
    {
        public IDependencyContainer Container { get; }

        public IAttributeParameterDefinitionProvider<Tuple<PropertyInfo, object>, Type> AttributeParameterProvider { get; }

        public IAttributeDefinitionProvider<Attribute, Type> AttributeProvider { get; }

        public IClassDefinitionProvider<Type, Type> ClassProvider { get; }

        public IStructDefinitionProvider<Type, Type> StructProvider { get; }

        public IEnumDefinitionProvider<Type, Type> EnumProvider { get; }

        public IEnumValueDefinitionProvider<object, Type> EnumValueProvider { get; }

        public IMethodDefinitionProvider<MethodInfo, Type> MethodProvider { get; }

        public IParameterDefinitionProvider<ParameterInfo, Type> ParameterProvider { get; }

        public IPropertyDefinitionProvider<PropertyInfo, Type> PropertyProvider { get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerFixture"/> class.
        /// </summary>
        public ContainerFixture()
        {
            var builder = DependencyBuilderFactory.Create(DependencyLibrary.Microsoft);

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
            builder.RegisterInstance<ILoggingService>(new LoggingService());
            builder.RegisterScoped<IInputService, InputService>();

            this.Container = builder.Build();

            this.AttributeProvider = this.Container.Resolve<IAttributeDefinitionProvider<Attribute, Type>>();
            this.AttributeParameterProvider = this.Container.Resolve<IAttributeParameterDefinitionProvider<Tuple<PropertyInfo, object>, Type>>();
            this.ClassProvider = this.Container.Resolve<IClassDefinitionProvider<Type, Type>>();
            this.EnumProvider = this.Container.Resolve<IEnumDefinitionProvider<Type, Type>>();
            this.EnumValueProvider = this.Container.Resolve<IEnumValueDefinitionProvider<object, Type>>();
            this.MethodProvider = this.Container.Resolve<IMethodDefinitionProvider<MethodInfo, Type>>();
            this.ParameterProvider = this.Container.Resolve<IParameterDefinitionProvider<ParameterInfo, Type>>();
            this.PropertyProvider = this.Container.Resolve<IPropertyDefinitionProvider<PropertyInfo, Type>>();
            this.StructProvider = this.Container.Resolve<IStructDefinitionProvider<Type, Type>>();

            this.ContainerBuilt();
        }

        private void ContainerBuilt()
        {
            this.AfterBuildContainer();
        }

        protected virtual void AfterBuildContainer()
        {

        }

        public void Dispose()
        {
            this.Container.Dispose();
        }
    }
}