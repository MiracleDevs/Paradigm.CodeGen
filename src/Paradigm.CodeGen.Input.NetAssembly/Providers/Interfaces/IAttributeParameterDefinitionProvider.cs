using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IAttributeParameterDefinitionProvider<in TSource, out TKey>: IDefinitionProvider<AttributeParameterDefinition, TSource, TKey>
    {
        
    }
}