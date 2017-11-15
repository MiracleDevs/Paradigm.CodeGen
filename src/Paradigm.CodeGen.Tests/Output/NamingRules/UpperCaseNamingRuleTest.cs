using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.NamingRules
{
    public class UpperCaseNamingRuleTest
    {
        #region Properties

        private UpperCaseNamingRule NamingRule { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.NamingRule = new UpperCaseNamingRule();
        }

        #endregion

        #region Public Methods

        public void ShouldFailWithInvalidParameters(string[] param)
        {
            var conf = new NamingRuleConfiguration { Parameters = new[] { "FirstParam", "SecondParam" } };
            Action execute = () => this.NamingRule.Execute("Name", conf);

            execute.Should().Throw<Exception>().WithMessage("Upper Case rule does not have parameters.");
        }

        [Test]
        public void ShouldUpperString()
        {
            this.NamingRule.Execute("TestingABCdef123", new NamingRuleConfiguration()).Should().Be("TESTINGABCDEF123");
        }

        #endregion
    }
}