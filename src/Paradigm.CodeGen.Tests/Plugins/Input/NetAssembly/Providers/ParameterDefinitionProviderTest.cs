using System;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using Paradigm.Core.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class ParameterDefinitionProviderTest
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
            var allParameters = attribute.GetType()
                                    .GetMethods()
                                    .SelectMany(x => x.GetParameters());

            foreach (var parameter in allParameters)
            {
                var processed = this.ContainerFixture.ParameterProvider.GetFromSource(parameter);
                processed.Name.Should().Be(parameter.Name);
            }
        }

        [Test]
        public void ShouldEqualNameAttributesAndTypeInProcessMethod()
        {
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();
            var attribute = new TestClass();
            var allParameters = attribute.GetType()
                                    .GetMethods()
                                    .SelectMany(x => x.GetParameters());

            foreach (var parameter in allParameters)
            { 
                var processed = this.ContainerFixture.ParameterProvider.Process(this.ContainerFixture.ParameterProvider.GetFromSource(parameter), objectDefinitions, parameter);
                processed.Name.Should().Be(parameter.Name);
                processed.Type.FullName.Should().Equals(parameter.ParameterType.GetReadableFullName());
                processed.Attributes.Count.Should().Equals(parameter.CustomAttributes.Count());
            }
        }

        #endregion

    }
}