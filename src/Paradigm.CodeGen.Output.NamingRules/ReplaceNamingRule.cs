using System;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public class ReplaceNamingRule : INamingRule
    {
        public string Execute(string name, NamingRuleConfiguration configuration)
        {
            if (configuration.Parameters.Length != 2)
                throw new Exception("Replace rule Has only 2 arguments, the string to be replaced, and the value to replace for.");

            return name.Replace(configuration.Parameters[0], configuration.Parameters[1]);
        }
    }
}