using System;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public class LowerCaseNamingRule : INamingRule
    {
        public string Execute(string name, NamingRuleConfiguration configuration)
        {
            if (configuration.Parameters.Length != 0)
                throw new Exception("Lower Case rule does not have parameters.");

            return name.ToLowerInvariant();
        }
    }
}