using System;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public class SeparateWordsNamingRule : INamingRule
    {
        public string Execute(string name, NamingRuleConfiguration configuration)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("Separate Words rule has only 1 argument, the separator string.");

            var separator = configuration.Parameters[0];
            var result = string.Empty;

            for (var index = 0; index < name.Length; index++)
            {
                var c = name[index];

                if (char.IsUpper(c) && index != 0)
                {
                    result += separator;
                }

                result += c;
            }

            return result;
        }
    }
}