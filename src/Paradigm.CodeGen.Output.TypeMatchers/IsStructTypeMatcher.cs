using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.TypeMatchers
{
    public class IsStructTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length != 0)
                throw new Exception("Is Struct type matcher hasn't any arguments.");

            return objectDefinition.GetType() == typeof(StructDefinition);
        }
    }
}