using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class FullNameEndsWithTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private FullNameEndsWithTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new FullNameEndsWithTypeMatcher();
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

            match.ShouldThrow<Exception>().WithMessage("FullNameEndsWith type matcher needs at least 1 parameter.");
        }

        [Test]
        public void ShouldNotMatchIfStringIsSuffixOfClassFullName()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.EndsWith" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("FullNameEndsWithTypeMatcher must return False if param is not suffix of TestClass full name");
        }

        [Test]
        public void ShouldMatchIfStringIsSuffixOfClassFullName()
        { 
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.FullName.Substring(this.Fixture.ClassDefinition.FullName.Length / 2) } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("FullNameEndsWithTypeMatcher must return True if param is suffix of TestClass full name");
        }

        #endregion
    }
}