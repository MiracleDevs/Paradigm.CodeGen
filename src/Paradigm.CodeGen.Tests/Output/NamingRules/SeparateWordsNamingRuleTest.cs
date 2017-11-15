using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.NamingRules
{
    public class SeparateWordsNamingRuleTest
    {
        #region Properties

        private SeparateWordsNamingRule NamingRule { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.NamingRule = new SeparateWordsNamingRule();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new string[] { } })]
        [TestCase(new object[] { new[] { "FirstParam", "SecondParam" } })]
        public void ShouldFailWithInvalidParameters(string[] param)
        {
            var conf = new NamingRuleConfiguration { Parameters = param };
            Action execute = () => this.NamingRule.Execute("Name", conf);

            execute.Should().Throw<Exception>().WithMessage("Separate Words rule has only 1 argument, the separator string.");
        }

        [Test]
        public void ShouldSeparateWithDash()
        {
            var conf = new NamingRuleConfiguration { Parameters = new[] { "-" } };

            this.NamingRule.Execute("SeparateWordsNaming", conf).Should().Be("Separate-Words-Naming");
        }

        [Test]
        public void ShouldSeparateWithMultipleChars()
        {
            var conf = new NamingRuleConfiguration { Parameters = new[] { "xx" } };

            this.NamingRule.Execute("SeparateWordsNaming", conf).Should().Be("SeparatexxWordsxxNaming");
        }

        #endregion
    }
}