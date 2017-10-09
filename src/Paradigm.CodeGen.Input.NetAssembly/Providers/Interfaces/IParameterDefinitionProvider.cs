using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces
{
    public interface IParameterDefinitionProvider<in TSource, out TKey> : IDefinitionProvider<ParameterDefinition, TSource, TKey>
    {
    }
}