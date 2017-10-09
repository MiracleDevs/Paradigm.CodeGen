using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NamespaceEqualsTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NamespaceEqualsTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NamespaceEqualsTypeMatcher();
        }

        #endregion

        #region TearDown

        public void TearDown()
        {
            this.Fixture.Dispose();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new[] { "Param1", "Param2" } })]
        [TestCase(new object[] { new string[] { } })]
        public void ThrowWhenParametersAreInvalid(string[] param)
        {
            var config = new TypeMatcherConfiguration { Parameters = param };
            Action match = () => this.TypeMatcher.Match(config, this.Fixture.ClassDefinition);

            match.ShouldThrow<Exception>().WithMessage("NamespaceEquals type matcher has only 1 argument, the string to be found.");
        }

        [Test]
        public void ShouldNotMatchIfStringEqualsToClassNamespace()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "FakeName1" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("Must return True only when objectDefinition's namespace is the only params");
        }

        [Test]
        public void ShouldMatchIfStringEqualsToClassNamespace()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Namespace } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("Must return False only when objectDefinition's namespace is not in params");
        }

        #endregion
    }
}