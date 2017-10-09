using System;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;
using FluentAssertions;
using System.Linq;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class ClassDefinitionProviderTest
    {
        private ContainerFixture ContainerFixture { get; set; }

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.ContainerFixture = new ContainerFixture();
        }

        #endregion

        #region TearDown

        [OneTimeTearDown]
        public void TearDown()
        {
            this.ContainerFixture.Dispose();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ClassDefinitionProvider_GetFromSource()
        {
            //Act
            var result = this.ContainerFixture.ClassProvider.GetFromSource(typeof(TestAbstractClass)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(TestAbstractClass));
            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(0);
            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(0);
            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(0);
        }

        [Test]
        public void ClassDefinitionProvider_Process()
        {
            //Act
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();
            var result = this.ContainerFixture.ClassProvider.Process(this.ContainerFixture.ClassProvider.GetFromSource(typeof(TestClass)), objectDefinitions, typeof(TestClass)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(TestClass));
            result.IsAbstract.Should().BeFalse();
            result.IsArray.Should().BeFalse();
            result.IsInterface.Should().BeFalse();

            result.ImplementedInterfaces.Should().NotBeNull();
            result.ImplementedInterfaces.Count.Should().Be(0);

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(3);

            result.Properties.Select(x => x.Name).Contains("InternalItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticItems").Should().BeFalse();

            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(3);

            result.Properties.Select(x => x.Name).Contains("InternalMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticMethod").Should().BeFalse();

            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(1);
        }

        [Test]
        public void ClassDefinitionProvider_ProcessImplements()
        {
            //Arrange
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.ClassProvider.Process(this.ContainerFixture.ClassProvider.GetFromSource(typeof(TestClassImplements)), objectDefinitions, typeof(TestClassImplements)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(TestClassImplements));
            result.IsAbstract.Should().BeFalse();
            result.IsArray.Should().BeFalse();
            result.IsInterface.Should().BeFalse();
            
            result.ImplementedInterfaces.Should().NotBeNull();
            result.ImplementedInterfaces.Count.Should().Be(2);

            result.ImplementedInterfaces.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));

            result.ImplementedInterfaces[0].Name.Should().Be(nameof(IDisposable));
            result.ImplementedInterfaces[0].FullName.Should().Be(typeof(IDisposable).FullName);

            result.ImplementedInterfaces[1].Name.Should().Be(nameof(ITestClass));
            result.ImplementedInterfaces[1].FullName.Should().Be(typeof(ITestClass).FullName);

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(2);

            result.Properties.Select(x => x.Name).Contains("InternalItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticItems").Should().BeFalse();
            
            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(3);

            result.Properties.Select(x => x.Name).Contains("InternalMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticMethod").Should().BeFalse();

            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(1);
        }

        [Test]
        public void ClassDefinitionProvider_ProcessAbstract()
        {
            //Arrange
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.ClassProvider.Process(this.ContainerFixture.ClassProvider.GetFromSource(typeof(TestAbstractClass)), objectDefinitions, typeof(TestAbstractClass)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(TestAbstractClass));
            result.IsAbstract.Should().BeTrue();
            result.IsArray.Should().BeFalse();
            result.IsInterface.Should().BeFalse();

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(2);

            result.ImplementedInterfaces.Should().NotBeNull();
            result.ImplementedInterfaces.Count.Should().Be(0);
            
            result.Properties.Select(x => x.Name).Contains("InternalItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticItems").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticItems").Should().BeFalse();

            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(2);          

            result.Properties.Select(x => x.Name).Contains("InternalMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticMethod").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticMethod").Should().BeFalse();

            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(1);
        }

        [Test]
        public void ClassDefinitionProvider_ProcessExtends()
        {
            //Arrange
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.ClassProvider.Process(this.ContainerFixture.ClassProvider.GetFromSource(typeof(TestClassExtends)), objectDefinitions, typeof(TestClassExtends)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(TestClassExtends));
            result.IsAbstract.Should().BeFalse();
            result.IsArray.Should().BeFalse();
            result.IsInterface.Should().BeFalse();

            result.ImplementedInterfaces.Should().NotBeNull();
            result.ImplementedInterfaces.Count.Should().Be(0);

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(2);

            result.Properties.Select(x => x.Name).Contains("InternalItemsExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedItemsExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateItemsExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticItemsExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticItemsExtended").Should().BeFalse();
            
            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(2);

            result.Properties.Select(x => x.Name).Contains("InternalMethodExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedMethodExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateMethodExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("ProtectedStaticMethodExtended").Should().BeFalse();
            result.Properties.Select(x => x.Name).Contains("PrivateStaticMethodExtended").Should().BeFalse();

            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(1);
        }

        [Test]
        public void ClassDefinitionProvider_ProcessInterface()
        {
            //Arrange
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.ClassProvider.Process(this.ContainerFixture.ClassProvider.GetFromSource(typeof(ITestClass)), objectDefinitions, typeof(ITestClass)) as ClassDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(ITestClass));
            result.IsAbstract.Should().BeTrue();
            result.IsArray.Should().BeFalse();
            result.IsInterface.Should().BeTrue();

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(1);

            result.ImplementedInterfaces.Should().NotBeNull();
            result.ImplementedInterfaces.Count.Should().Be(0);

            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(1);
        }

        #endregion
    }
}