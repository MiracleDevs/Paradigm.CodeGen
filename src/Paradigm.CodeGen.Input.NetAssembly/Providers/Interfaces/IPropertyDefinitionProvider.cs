using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IPropertyDefinitionProvider<in TSource, out TKey> : IDefinitionProvider<PropertyDefinition, TSource, TKey>
    {
    }
}