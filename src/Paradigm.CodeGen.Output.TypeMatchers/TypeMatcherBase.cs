using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public abstract class TypeMatcherBase : ITypeMatcher
    {
        public bool Match(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Negate)
                return !this.IsMatch(configuration, objectDefinition);

            return this.IsMatch(configuration, objectDefinition);
        }

        protected abstract bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition);
    }
}