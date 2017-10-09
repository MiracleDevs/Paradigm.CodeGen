using System;
using System.Collections.Generic;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces
{
    public interface IObjectDefinitions<in TKey>
    {
        void Add(TKey key, ObjectDefinitionBase definition);

        bool ContainsDefinition(TKey key);

        ObjectDefinitionBase Get(TKey key);

        IEnumerable<ObjectDefinitionBase> Find(Func<ObjectDefinitionBase, bool> predicate = null);
    }
}