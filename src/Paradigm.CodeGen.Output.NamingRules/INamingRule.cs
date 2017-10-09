using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public interface INamingRule
    {
        string Execute(string name, NamingRuleConfiguration configuration);
    }
}