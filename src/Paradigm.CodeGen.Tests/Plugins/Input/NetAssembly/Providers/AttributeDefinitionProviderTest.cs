using System;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class AttributeDefinitionProviderTest
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
        public void AttributeProvider_GetFromSource()
        {
            //Arrange
            var attribute = new MockBaseAttribute();

            //Act
            var result = this.ContainerFixture.AttributeProvider.GetFromSource(attribute);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(MockBaseAttribute));
            result.Parameters.Should().NotBeNull();
            result.Parameters.Count.Should().Be(0);
        }

        [Test]
        public void AttributeProvider_Process()
        {
            //Arrange
            var attribute = new MockBaseAttribute();
            IObjectDefinitions<Type> objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.AttributeProvider.Process(this.ContainerFixture.AttributeProvider.GetFromSource(attribute), objectDefinitions, attribute);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(MockBaseAttribute));
            result.Parameters.Should().NotBeNull();
            result.Parameters.Count.Should().Be(5);

            //Parameters Assertion
            result.Parameters.Select(x => x.Name).Contains("PrivateAttr").Should().BeFalse();
            result.Parameters.Select(x => x.Name).Contains("ProtectedAttr").Should().BeFalse();
        }

        [Test]
        public void AttributeProvider_ProcessInherance()
        {
            //Arrange
            var attribute = new MockAttribute();
            IObjectDefinitions<Type> objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            //Act
            var result = this.ContainerFixture.AttributeProvider.Process(this.ContainerFixture.AttributeProvider.GetFromSource(attribute), objectDefinitions, attribute);
            
            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(nameof(MockAttribute));
            result.Parameters.Should().NotBeNull();
            result.Parameters.Count.Should().Be(6);

            //Parameters Assertion
            result.Parameters.Select(x => x.Name).Contains("PrivateAttr").Should().BeFalse();
            result.Parameters.Select(x => x.Name).Contains("PrivateAttr2").Should().BeFalse();
            result.Parameters.Select(x => x.Name).Contains("ProtectedAttr").Should().BeFalse();
            result.Parameters.Select(x => x.Name).Contains("ProtectedAttr2").Should().BeFalse();
        }

        #endregion
    }
}