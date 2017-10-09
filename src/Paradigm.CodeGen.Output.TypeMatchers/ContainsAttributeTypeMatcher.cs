using System;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public class ContainsAttributeTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("Contains attribute type matcher has only 1 argument, the string to be found.");

            return objectDefinition.Attributes.Any(x => x.Name == configuration.Parameters[0]);
        }
    }
}