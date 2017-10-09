using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IEnumValueDefinitionProvider<in TSource, out TKey> : IDefinitionProvider<EnumValueDefinition, TSource, TKey>
    {
    }
}