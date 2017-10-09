using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public class IsEnumTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 0)
                throw new Exception("Is Enum type matcher hasn't any arguments.");

            return objectDefinition.GetType() == typeof(EnumDefinition);
        }
    }
}