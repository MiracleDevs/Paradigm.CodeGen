using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{   
    public class IsEnumTypeMatcherTest
    {
        #region Properties

        private EnumDefinition EnumDefinition { get; set; }

        private IsEnumTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.EnumDefinition = new EnumDefinition();
            this.TypeMatcher = new IsEnumTypeMatcher();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Param1", "Param2" } };
            Action match = () => this.TypeMatcher.Match(config, this.EnumDefinition);

            match.Should().Throw<Exception>().WithMessage("Is Enum type matcher hasn't any arguments.");
        }

        [Test]
        public void ShouldNotMatchIfProcessedObjectIsNotAEnumDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, new ClassDefinition()).Should().BeFalse("Should return True only when objectDefinition is EnumDefinition");
        }

        [Test]
        public void ShouldMatchIfProcessedObjectIsAEnumDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, this.EnumDefinition).Should().BeTrue("Should return False when objectDefinition is not EnumDefinition");
        }

        #endregion
    }
}