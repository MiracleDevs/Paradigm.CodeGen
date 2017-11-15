using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    public class IsArrayTypeMatcherTest
    {
        #region Properties

        private StructDefinition ArrayDefinition { get; set; }

        private IsArrayTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.ArrayDefinition = new StructDefinition { IsArray = true };
            this.TypeMatcher = new IsArrayTypeMatcher();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Param1", "Param2" } };
            Action match = () => this.TypeMatcher.Match(config, this.ArrayDefinition);

            match.Should().Throw<Exception>().WithMessage("Is Array type matcher hasn't any arguments.");
        }

        [Test]
        public void ShouldNotMatchIfProcessedStructIsNotAnArray()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, new EnumDefinition()).Should().BeFalse("Must return True only when objectDefinition.IsArray is True");
        }

        [Test]
        public void ShouldMatchIfProcessedStructIsAnArray()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, this.ArrayDefinition).Should().BeTrue("Must return False only when objectDefinition.IsArray is False");
        }

        #endregion
    }
}