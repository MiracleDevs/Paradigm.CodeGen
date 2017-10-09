using System;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public class UpperCaseNamingRule : INamingRule
    {
        public string Execute(string name, NamingRuleConfiguration configuration)
        {
            if (configuration.Parameters.Length != 0)
                throw new Exception("Upper Case rule does not have parameters.");

            return name.ToUpperInvariant();
        }
    }
}