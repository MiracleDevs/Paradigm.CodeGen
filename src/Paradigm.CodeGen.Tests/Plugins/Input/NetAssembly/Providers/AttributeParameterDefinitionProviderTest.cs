using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class AttributeParameterDefinitionProviderTest
    {
        private ContainerFixture ContainerFixture { get; set; }

        private string[] NumericTypes { get; set; }

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.ContainerFixture = new ContainerFixture();
            this.NumericTypes = new string[] 
                           {
                               typeof(int).FullName,
                               typeof(uint).FullName,
                               typeof(long).FullName,
                               typeof(ulong).FullName,
                               typeof(short).FullName,
                               typeof(ushort).FullName,
                               typeof(byte).FullName,
                               typeof(sbyte).FullName,
                               typeof(float).FullName,
                               typeof(double).FullName,
                               typeof(decimal).FullName
                           };
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
        public void ShouldEqualNameValueAndIsNumericInGetFromSourcesMethod()
        {
            var attribute = new MockBaseAttribute();
            var type = attribute.GetType();

            type.Should().NotBeNull();

            var typeInfo = type.GetTypeInfo();

            foreach (var property in typeInfo.DeclaredProperties)
            {
                var tuple = new Tuple<PropertyInfo, object>(property, attribute);
                var processed = this.ContainerFixture.AttributeParameterProvider.GetFromSource(tuple);
                processed.Name.Should().Be(property.Name);
                var propaVal = property.GetValue(attribute);

                if (propaVal == null)
                    processed.Value.Should().BeNull();
                else
                    processed.Value.Should().Be(propaVal.ToString());

                processed.IsNumeric.Should().Be(this.IsNumeric(property));
            }
        }

        [Test]
        public void ShouldEqualNameValueAndIsNumericInProcessMethod()
        { 
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();
            var attribute = new MockBaseAttribute();
            var type = attribute.GetType();

            type.Should().NotBeNull();

            var typeInfo = type.GetTypeInfo();

            foreach (var property in typeInfo.DeclaredProperties)
            {
                var tuple = new Tuple<PropertyInfo, object>(property, attribute);
                var processed = this.ContainerFixture.AttributeParameterProvider.Process(this.ContainerFixture.AttributeParameterProvider.GetFromSource(tuple), objectDefinitions, tuple);
                processed.Name.Should().Be(property.Name);
                var propaVal = property.GetValue(attribute);

                if (propaVal == null)
                    processed.Value.Should().BeNull();
                else 
                    processed.Value.Should().Be(propaVal.ToString());

                processed.IsNumeric.Should().Be(this.IsNumeric(property));
            }
        }

        private bool IsNumeric(PropertyInfo property)
        {
            return this.NumericTypes.Contains(property.PropertyType.FullName);
        }

        #endregion
    }
}