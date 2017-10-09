using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class NameInTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private NameInTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new NameInTypeMatcher();
        }

        #endregion

        #region TearDown

        public void TearDown()
        {
            this.Fixture.Dispose();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration();
            Action match = () => this.TypeMatcher.Match(config, this.Fixture.ClassDefinition);

            match.ShouldThrow<Exception>().WithMessage("NameIn type expect an array of names.");
        }

        [Test]
        public void ShouldNotMatchIfClassNameIsNotInParameterList()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "FakeName1", "FakeName2" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeFalse("Must return True only when objectDefinition's name is in params");
        }

        [Test]
        public void ShouldMatchIfClassNameIsInParameterList()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { this.Fixture.ClassDefinition.Name, "FakeName2" } };

            this.TypeMatcher.Match(config, this.Fixture.ClassDefinition).Should().BeTrue("Must return False only when objectDefinition's name is not in params");
        }

        #endregion
    }
}