using System;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.TypeMatchers
{
    public class NamespaceStartsWithTypeMatcher : TypeMatcherBase
    {
        protected override bool IsMatch(TypeMatcherConfiguration configuration, ObjectDefinitionBase objectDefinition)
        {
            if (configuration.Parameters.Length < 1)
                throw new Exception("NamespaceStartsWith type matcher needs at least 1 parameter.");

            return configuration.Parameters.Aggregate(false, (current, parameter) => current || objectDefinition.Namespace.StartsWith(parameter));
        }
    }
}