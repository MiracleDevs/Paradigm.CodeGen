using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NameEndsWithTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NameEndsWithTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NameEndsWithTypeMatcher();
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

            match.Should().Throw<Exception>().WithMessage("NameEndsWith type matcher needs at least 1 parameter.");
        }

        [Test]
        public void ShouldNotMatchIfStringIsSuffixOfClassName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.EndsWith" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("NameEndsWithTypeMatcher must return False if param is not suffix of TestClass name");
        }

        [Test]
        public void ShouldMatchIfStringIsSuffixOfClassName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Name.Substring(this.Fixture.ClassDefinition.Name.Length / 2) } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("NameEndsWithTypeMatcher must return True if param is suffix of TestClass name");
        }

        #endregion
    }
}