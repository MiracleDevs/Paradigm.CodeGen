using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Output.TypeMatchers
{
    [TestFixture]
    public class InheritsFromTypeMatcherTest
    {
        #region Properties

        private TypeMatcherFixture Fixture { get; set; }

        private InheritsFromTypeMatcher TypeMatcher { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture = new TypeMatcherFixture();
            this.TypeMatcher = new InheritsFromTypeMatcher();
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

            match.Should().Throw<Exception>().WithMessage("Inherits From type matcher has only 1 argument, the string to be found.");
        }

        [Test]
        public void ShouldNotMatchIfStringIsNotSuperClassOfClass()
        {
            var classDefinition = this.Fixture.ClassProvider.Process(this.Fixture.ClassProvider.GetFromSource(typeof(TestClassExtends)), this.Fixture.ObjectDefinitions, typeof(TestClassExtends)) as ClassDefinition;

            classDefinition.Should().NotBeNull();

            var config = new TypeMatcherConfiguration { Parameters = new[] { "FakeNonExistantClass" } };

            this.TypeMatcher.Match(config, classDefinition).Should().BeFalse("InheritsFromTypeMatcher must return False if param is not basetype of TestClassExtends");
        }

        [Test]
        public void ShouldMatchIfStringIsSuperClassOfClass()
        {
            var classDefinition = this.Fixture.ClassProvider.Process(this.Fixture.ClassProvider.GetFromSource(typeof(TestClassExtends)), this.Fixture.ObjectDefinitions, typeof(TestClassExtends)) as ClassDefinition;

            classDefinition.Should().NotBeNull();

            var config = new TypeMatcherConfiguration { Parameters = new[] { "TestClass" } };

            this.TypeMatcher.Match(config, classDefinition).Should().BeTrue("InheritsFromTypeMatcher must return True if param is basetype of TestClassExtends");
        }

        #endregion
    }
}