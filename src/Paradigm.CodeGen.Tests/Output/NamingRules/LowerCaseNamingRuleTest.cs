using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.NamingRules
{
    public class LowerCaseNamingRuleTest
    {
        #region Properties

        private LowerCaseNamingRule NamingRule { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.NamingRule = new LowerCaseNamingRule();
        }

        #endregion

        #region Public Methods

        public void ShouldFailWithInvalidParameters(string[] param)
        {
            var conf = new NamingRuleConfiguration { Parameters = new[]{ "FirstParam", "SecondParam" }};
            Action execute = () => this.NamingRule.Execute("Name", conf);

            execute.ShouldThrow<Exception>().WithMessage("Lower Case rule does not have parameters.");
        }

        [Test]
        public void ShouldLowerString()
        {
            this.NamingRule.Execute("TestingABCdef123", new NamingRuleConfiguration()).Should().Be("testingabcdef123");
        }

        #endregion
    }
}