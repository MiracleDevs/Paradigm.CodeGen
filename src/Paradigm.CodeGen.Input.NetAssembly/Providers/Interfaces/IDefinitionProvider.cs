using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IDefinitionProvider<TDefinition, in TSource, out TKey>
    {
        TDefinition GetFromSource(TSource parameter);

        TDefinition Process(TDefinition definition, IObjectDefinitions<TKey> objectDefinitions, TSource parameter);
    }
}