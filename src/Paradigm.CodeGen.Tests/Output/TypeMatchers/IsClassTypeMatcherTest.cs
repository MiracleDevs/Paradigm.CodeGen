using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    public class IsClassTypeMatcherTest
    {
        #region Properties

        private ClassDefinition ClassDefinition { get; set; }

        private IsClassTypeMatcher TypeMatcher { get; set;  }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.ClassDefinition = new ClassDefinition();
            this.TypeMatcher = new IsClassTypeMatcher();
        }

        #endregion

        #region Public Methods

        [Test]
        public void ThrowWhenParametersAreInvalid()
        {
            var config = new TypeMatcherConfiguration { Parameters = new[] { "Param1", "Param2" } };
            Action match = () => this.TypeMatcher.Match(config, this.ClassDefinition);

            match.Should().Throw<Exception>().WithMessage("Is Class type matcher hasn't any arguments.");
        }

        [Test]
        public void ShouldNotMatchIfProcessedObjectIsNotAClassDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, new StructDefinition()).Should().BeFalse("Must return True only when objectDefinition is ClassDefinition");
        }

        [Test]
        public void ShouldMatchIfProcessedObjectIsAClassDefinition()
        {
            var config = new TypeMatcherConfiguration();

            this.TypeMatcher.Match(config, this.ClassDefinition).Should().BeTrue("Must return False when objectDefinition is not ClassDefinition");
        }

        #endregion
    }
}