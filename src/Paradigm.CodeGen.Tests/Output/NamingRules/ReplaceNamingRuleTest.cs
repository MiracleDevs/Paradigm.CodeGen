using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.NamingRules
{
    public class ReplaceNamingRuleTest
    {
        #region Properties

        private ReplaceNamingRule NamingRule { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.NamingRule = new ReplaceNamingRule();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new string[] { } })]
        [TestCase(new object[] { new[] { "OneParam" } })]
        [TestCase(new object[] { new[] { "FirstParam", "SecondParam", "ThirdParam" } })]
        public void ShouldFailWithInvalidParameters(string[] param)
        {
            var conf = new NamingRuleConfiguration { Parameters = param };
            Action execute = () => this.NamingRule.Execute("Name", conf);

            execute.Should().Throw<Exception>().WithMessage("Replace rule Has only 2 arguments, the string to be replaced, and the value to replace for.");
        }

        [Test]
        public void ShouldReplaceString()
        {
            var conf = new NamingRuleConfiguration { Parameters = new[] { "REPLACE", "ReplaceNaming" } };

            this.NamingRule.Execute("TestingREPLACE", conf).Should().Be("TestingReplaceNaming");
        }

        #endregion
    }
}
