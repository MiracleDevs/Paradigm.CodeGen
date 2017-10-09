using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IAttributeDefinitionProvider<in TSource, out TKey>: IDefinitionProvider<AttributeDefinition, TSource, TKey>
    {        
    }
}