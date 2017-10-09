using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using Paradigm.Core.Extensions;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class PropertyDefinitionProviderTest
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
        public void ShouldEqualNameInGetFromSourcesMethod()
        {
            var attribute = new TestClass();
            var type = attribute.GetType();

            type.Should().NotBeNull();

            var typeInfo = type.GetTypeInfo();

            foreach (var property in typeInfo.DeclaredProperties)
            {
                var processed = this.ContainerFixture.PropertyProvider.GetFromSource(property);
                processed.Name.Should().Be(property.Name);
            }
        }

        [Test]
        public void ShouldEqualNameAttributesAndTypeInProcessMethod()
        {
            IObjectDefinitions<Type> objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();
            var attribute = new TestClass();
            var type = attribute.GetType();

            type.Should().NotBeNull();

            var typeInfo = type.GetTypeInfo();

            foreach (var property in typeInfo.DeclaredProperties)
            { 
                var processed = this.ContainerFixture.PropertyProvider.Process(this.ContainerFixture.PropertyProvider.GetFromSource(property), objectDefinitions, property);
                processed.Name.Should().Be(property.Name);
                processed.Attributes.Count.Should().Equals(property.CustomAttributes.Count());
                processed.Type.FullName.Should().Be(property.PropertyType.GetReadableFullName());
            }
        }

        #endregion
    }
}