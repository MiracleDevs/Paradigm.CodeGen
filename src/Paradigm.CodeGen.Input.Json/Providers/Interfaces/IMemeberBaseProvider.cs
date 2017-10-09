using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Interfaces
{
    public interface IMemeberBaseProvider<in TInput, TOutput> : IDefinitionProvider<TInput, TOutput>
        where TInput : MemberBase
        where TOutput : MemberDefinitionBase
    {

    }
}