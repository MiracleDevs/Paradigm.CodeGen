using System;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Extensions
{
    public static class ObjectDefinitionBaseExtensions
    {
        public static bool InheritsFrom(this ObjectDefinitionBase definition, string name)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return definition.FindInHierarchy(name) != null;
        }

        private static ObjectDefinitionBase FindInHierarchy(this ObjectDefinitionBase definition, string name, bool includeItself = false)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if ((definition.FullName == name || definition.Name == name) && includeItself)
                return definition;

            // search first in the base class hierarchy.
            if (definition.BaseType != null)
            {
                var result = FindInHierarchy(definition.BaseType, name, true);

                if (result != null)
                {
                    return result;
                }
            }

            // then search in each implemented interface hierarchy.
            if (definition.ImplementedInterfaces != null && definition.ImplementedInterfaces.Any())
            {
                foreach(var implemented in definition.ImplementedInterfaces)
                {
                    var result = FindInHierarchy(implemented, name, true);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}