using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.TypeMatchers
{
    public class FullNameEqualsTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("FullNameEquals type matcher has only 1 argument, the string to be found.");

            return objectDefinition.FullName.Equals(configuration.Parameters[0]);
        }
    }
}