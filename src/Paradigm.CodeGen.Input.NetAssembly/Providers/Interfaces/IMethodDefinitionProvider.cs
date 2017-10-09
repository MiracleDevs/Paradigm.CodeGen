using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IMethodDefinitionProvider<in TSource, out TKey> : IDefinitionProvider<MethodDefinition, TSource, TKey>
    {
    }
}