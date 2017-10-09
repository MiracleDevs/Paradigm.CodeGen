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
    public class MethodDefinitionProviderTest
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
        public void ShouldEqualNameGetFromSourcesMethod()
        {
            var attribute = new TestClass();
            var allMethods = attribute.GetType().GetMethods();

            foreach (var method in allMethods)
            {
                var processed = this.ContainerFixture.MethodProvider.GetFromSource(method);
                processed.Name.Should().Be(method.Name);
            }
        }

        [Test]
        public void ShouldEqualNameAttributesParametersAndReturnTypeInProcessMethod()
        {
            
            IObjectDefinitions<Type> objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();
            var attribute = new TestClass();
            var allMethods = attribute.GetType().GetMethods();


            foreach (var method in allMethods)
            { 
                var processed = this.ContainerFixture.MethodProvider.Process(this.ContainerFixture.MethodProvider.GetFromSource(method), objectDefinitions, method);
                processed.Name.Should().Be(method.Name);
                processed.ReturnType.FullName.Should().Be(method.ReturnType.GetReadableFullName());
                processed.Attributes.Count.Should().Be(method.CustomAttributes.Count());
                processed.Parameters.Count.Should().Be(method.GetParameters().Count());
            }
        }

        #endregion
    }
}