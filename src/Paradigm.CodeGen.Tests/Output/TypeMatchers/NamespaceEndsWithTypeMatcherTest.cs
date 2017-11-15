using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NamespaceEndsWithTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NamespaceEndsWithTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NamespaceEndsWithTypeMatcher();
        }

        #endregion

        #region TearDown

        public void TearDown()
        {
            this.Fixture.Dispose();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new string[] { } })]
        public void ThrowWhenParametersAreInvalid(string[] param)
        {
            var config = new TypeMatcherConfiguration { Parameters = param };
            Action match = () => this.TypeMatcher.Match(config, this.Fixture.ClassDefinition);

            match.Should().Throw<Exception>().WithMessage("NamespaceEndsWith type matcher needs at least 1 parameter.");
        }

        [Test]
        public void ShouldNotMatchIfStringIsSuffixOfClassNamespace()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.EndsWith" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("NamespaceEndsWithTypeMatcher must return False if param is not suffix of TestClass namespace");
        }

        [Test]
        public void ShouldMatchIfStringIsSuffixOfClassNamespace()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Namespace.Substring(this.Fixture.ClassDefinition.Namespace.Length / 2) } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("NamespaceEndsWithTypeMatcher must return True if param is suffix of TestClass namespace");
        }

        #endregion
    }
}