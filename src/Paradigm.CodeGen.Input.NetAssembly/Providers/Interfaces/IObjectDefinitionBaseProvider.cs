using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IObjectDefinitionBaseProvider<in TSource, out TKey> : IDefinitionProvider<ObjectDefinitionBase, TSource, TKey> 
    {      
    }
}