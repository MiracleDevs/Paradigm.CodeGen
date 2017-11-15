using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NameEqualsTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NameEqualsTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NameEqualsTypeMatcher();
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

            match.Should().Throw<Exception>().WithMessage("NameEquals type matcher has only 1 argument, the string to be found.");
        }

        [Test]
        public void ShouldNotMatchIfStringEqualsToClassName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "FakeName1" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("Must return True only when objectDefinition's name is the only params");
        }

        [Test]
        public void ShouldMatchIfStringEqualsToClassName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Name } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("Must return False only when objectDefinition's name is not in params");
        }

        #endregion
    }
}