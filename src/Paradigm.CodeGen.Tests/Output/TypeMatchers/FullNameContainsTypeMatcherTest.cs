using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class FullNameContainsTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private FullNameContainsTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new FullNameContainsTypeMatcher();
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

            match.ShouldThrow<Exception>().WithMessage("FullNameContains type matcher has only 1 argument, the string to be found.");
        }
        
        [Test]
        public void ShouldNotMatchIfStringIsContainedInsideFullName()
        { 
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.Contains" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("FullNameContainsTypeMatcher must return True if param is a substring of TestClass full name");
        }

        [Test]
        public void ShouldMatchIfStringIsContainedInsideFullName()
        { 
            var quaterLenght = this.Fixture.ClassDefinition.FullName.Length / 4;
            var halfLenght = this.Fixture.ClassDefinition.FullName.Length / 2;
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.FullName.Substring(halfLenght - quaterLenght, halfLenght + quaterLenght) } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("FullNameContainsTypeMatcher must return False if param is not a substring of TestClass full name");
        }

        #endregion
    }
}
