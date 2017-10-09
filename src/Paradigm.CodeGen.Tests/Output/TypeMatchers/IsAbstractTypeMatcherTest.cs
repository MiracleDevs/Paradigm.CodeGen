using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    public class IsAbstractTypeMatcherTest
    {
        #region Properties

        private StructDefinition StructDefinition { get; set; }

        private IsAbstractTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.StructDefinition = new StructDefinition { IsAbstract = true };
            this.TypeMatcher = new IsAbstractTypeMatcher();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Param1", "Param2" } };
            Action match = () => this.TypeMatcher.Match(config, this.StructDefinition);

            match.ShouldThrow<Exception>().WithMessage("Is Abstract type matcher hasn't any arguments.");
        }

        [Test]
        public void ShouldNotMatchIfProcessedStructIsNotAbstract()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, new EnumDefinition()).Should().BeFalse("Must return True only when objectDefinition.IsAbstract is True");
        }

        [Test]
        public void ShouldMatchIfProcessedStructIsAbstract()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, this.StructDefinition).Should().BeTrue("Must return False only when objectDefinition.IsAbstract is False");
        }

        #endregion
    }
}