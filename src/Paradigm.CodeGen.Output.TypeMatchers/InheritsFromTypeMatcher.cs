using System;
using Paradigm.CodeGen.Extensions;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public class InheritsFromTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 1)
                throw new Exception("Inherits From type matcher has only 1 argument, the string to be found.");

            return objectDefinition.InheritsFrom(configuration.Parameters[0]);
        }
    }
}