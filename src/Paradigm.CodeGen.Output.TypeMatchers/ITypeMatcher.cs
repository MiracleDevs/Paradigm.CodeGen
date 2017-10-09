using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public interface ITypeMatcher
    {
        bool Match(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition);
    }
}