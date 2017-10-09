using System;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;
using FluentAssertions;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class StructDefinitionProviderTest
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
        public void StructDefinitionProvider_GetFromSource()
        { 
            //Act
            var result = this.ContainerFixture.StructProvider.GetFromSource(typeof(MockStructBase)) as StructDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(MockStructBase));
            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(0);
        }

        [Test]
        public void StructDefinitionProvider_Process()
        {
            //Arrange
            IObjectDefinitions<Type> objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.StructProvider.Process(this.ContainerFixture.StructProvider.GetFromSource(typeof(MockStructBase)), objectDefinitions, typeof(MockStructBase)) as StructDefinition;

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(MockStructBase));

            result.Properties.Should().NotBeNull();
            result.Properties.Count.Should().Be(3);
            
            result.Methods.Should().NotBeNull();
            result.Methods.Count.Should().Be(3);

            result.Attributes.Should().NotBeNull();
            result.Attributes.Count.Should().Be(1);
            result.Attributes[0].Name.Should().Be(nameof(MockAttribute));
        }

        #endregion

    }
}
