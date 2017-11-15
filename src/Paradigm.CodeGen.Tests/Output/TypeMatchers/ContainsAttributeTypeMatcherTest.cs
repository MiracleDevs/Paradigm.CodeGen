using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class ContainsAttributeTypeMatcherTest 
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set;  }

        private ContainsAttributeTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new ContainsAttributeTypeMatcher();
        }

        #endregion

        #region TearDown

        public void TearDown()
        {
            this.Fixture.Dispose();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new[] { "Class1", "Class2" } })]
        [TestCase(new object[] { new string[] { } })]
        public void ThrowWhenParametersAreInvalid(string[] param)
        {
            var config = new TypeMatcherConfiguration { Parameters = param };
            Action match = () => this.TypeMatcher.Match(config, this.Fixture.ClassDefinition);

            match.Should().Throw<Exception>().WithMessage("Contains attribute type matcher has only 1 argument, the string to be found.");
        }

        [Test]
        public void MatchNonExistantParam()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Testing.Contains" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("ContainsAttributeTypeMatcher must return True if param contains an attribute of TestClass");
        }

        [Test]
        public void MatchExistantParam()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Attributes[0].Name } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("ContainsAttributeTypeMatcher must return False if param does not contains an attribute of TestClass");
        }

        #endregion
    }
}