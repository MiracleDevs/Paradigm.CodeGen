using System;
using FluentAssertions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class EnumDefinitionProviderTest
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
        public void ShouldAttributesBeZero()
        {
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            var enumType = typeof(MockEnum);

            var result = this.ContainerFixture.EnumProvider.GetFromSource(enumType);

            result.Should().NotBeNull();
            enumType.Name.Should().Be(result.Name);
            enumType.FullName.Should().Be(result.FullName);
            result.ImplementedInterfaces.Count.Should().Be(0);
            result.IsArray.Should().Be(false);
            result.IsInterface.Should().Be(false);
            result.IsAbstract.Should().Be(false);
            result.Attributes.Count.Should().Be(0);
        }

        
        [TestCase(typeof(MockEnum))]
        [TestCase(typeof(MockEnumLong))]
        [TestCase(typeof(MockEnumULong))]
        [TestCase(typeof(MockEnumInt))]
        [TestCase(typeof(MockEnumUInt))]
        [TestCase(typeof(MockEnumShort))]
        [TestCase(typeof(MockEnumUShort))]
        [TestCase(typeof(MockEnumByte))]
        [TestCase(typeof(MockEnumSByte))]
        public void ShouldValuesCountBeCorrect(Type enumType)
        { 
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            var result = this.ContainerFixture.EnumProvider.Process(this.ContainerFixture.EnumProvider.GetFromSource(enumType), objectDefinitions, enumType) as EnumDefinition;

            result.Should().NotBeNull();
            enumType.Name.Should().Be(result.Name);
            enumType.FullName.Should().Be(result.FullName);
            result.ImplementedInterfaces.Count.Should().Be(0);
            result.IsArray.Should().Be(false);
            result.IsInterface.Should().Be(false);
            result.IsAbstract.Should().Be(false);
            result.Values.Count.Should().Be(Enum.GetValues(enumType).Length);
        }

        #endregion

    }
}