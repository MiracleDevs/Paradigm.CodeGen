using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NameStartsWithTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NameStartsWithTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NameStartsWithTypeMatcher();
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
            
            match.Should().Throw<Exception>().WithMessage("NameStartsWith type matcher needs at least 1 parameter.");
        }

        [Test]
        public void ShouldNotMatchIfStringIsNotPrefixOfName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.StartsWith" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("NameStartsWithTypeMatcher must return False if param is not prefix of TestClass name");
        }

        [Test]
        public void ShouldMatchIfStringIsPrefixOfName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Name.Substring(0, this.Fixture.ClassDefinition.Name.Length / 2) } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("NameStartsWithTypeMatcher must return True if param is prefix of TestClass name");
        }

        #endregion
    }
}