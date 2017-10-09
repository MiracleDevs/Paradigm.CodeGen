using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.TypeMatchers
{
    public class NameEqualsTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("NameEquals type matcher has only 1 argument, the string to be found.");

            return objectDefinition.Name.Equals(configuration.Parameters[0]);
        }
    }
}