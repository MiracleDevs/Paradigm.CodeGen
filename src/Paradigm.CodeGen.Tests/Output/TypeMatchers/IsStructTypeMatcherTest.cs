using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    public class IsStructTypeMatcherTest
    {
        #region Properties

        private StructDefinition StructDefinition { get; set; }

        private IsStructTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.StructDefinition = new StructDefinition();
            this.TypeMatcher = new IsStructTypeMatcher();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Param1", "Param2" } };
            Action match = () => this.TypeMatcher.Match(config, this.StructDefinition);

            match.ShouldThrow<Exception>().WithMessage("Is Struct type matcher hasn't any arguments.");
        }

        [Test]
        public void ShouldNotMatchIfProcessedObjectIsNotAStructDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, new ClassDefinition()).Should().BeFalse("Must return True only when objectDefinition is StructDefinition");
        }

        [Test]
        public void ShouldMatchIfProcessedObjectIsAStructDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, this.StructDefinition).Should().BeTrue("Must return False when objectDefinition is not StructDefinition");
        }

        #endregion
    }
}