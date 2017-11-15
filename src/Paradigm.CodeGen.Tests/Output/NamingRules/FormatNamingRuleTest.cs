using System;
using FluentAssertions;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using NUnit.Framework;

namespace Paradigm.CodeGen.Tests.Output.NamingRules
{
    public class FormatNamingRuleTest
    {
        #region Properties

        private FormatNamingRule NamingRule { get; set; }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.NamingRule = new FormatNamingRule();
        }

        #endregion

        #region Public Methods

        [TestCase(new object[] { new string[] { } })]
        [TestCase(new object[] { new[] { "FirstParam", "SecondParam" } })]
        public void ShouldFailWithInvalidParameters(string[] param)
        {
            var conf = new NamingRuleConfiguration { Parameters = param };

            this.NamingRule.Invoking(x => x.Execute("Name", conf)).Should().Throw<Exception>().WithMessage("Format rule has only 1 argument, the format string.");
        }

        [Test]
        public void ShouldFormatString()
        {
            var conf = new NamingRuleConfiguration { Parameters = new[] { "Testing{0}" } };

            this.NamingRule.Execute("FormatNaming", conf).Should().Be("TestingFormatNaming");
        }

        #endregion
    }
}
