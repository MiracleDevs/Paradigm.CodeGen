using System;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public class FormatNamingRule : INamingRule
    {
        public string Execute(string name, NamingRuleConfiguration configuration)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("Format rule has only 1 argument, the format string.");

            return string.Format(configuration.Parameters[0], name);
        }
    }
}