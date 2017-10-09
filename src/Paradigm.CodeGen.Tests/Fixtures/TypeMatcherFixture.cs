using System;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Mocks;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Fixtures
{
    public class TypeMatcherFixture : ContainerFixture
    {
        public IObjectDefinitions<Type> ObjectDefinitions { get; private set; }

        public StructDefinition StructDefinition { get; private set; }

        public ClassDefinition ClassDefinition { get; private set; }

        public EnumDefinition EnumDefinition { get; private set; }

        protected override void AfterBuildContainer()
        {
            base.AfterBuildContainer();
            this.ObjectDefinitions = this.Container.Resolve<IObjectDefinitions<Type>>();

            var classType = typeof(TestClass);
            var structType = typeof(MockStructBase);
            var enumType = typeof(MockEnumInt);

            this.ClassDefinition = this.ClassProvider.Process(this.ClassProvider.GetFromSource(classType), this.ObjectDefinitions, classType) as ClassDefinition;
            this.StructDefinition = this.StructProvider.Process(this.ClassProvider.GetFromSource(structType), this.ObjectDefinitions, structType) as StructDefinition;
            this.EnumDefinition = this.EnumProvider.Process(this.ClassProvider.GetFromSource(enumType), this.ObjectDefinitions, enumType) as EnumDefinition;            
        }
    }
}