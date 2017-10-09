namespace Paradigm.CodeGen.Input.Json.Providers.Interfaces
{
    public interface IDefinitionProvider<in TInput, TOutput>
    {
        TOutput Create(TInput input);

        TOutput Process(TOutput output, TInput input, IInputService service);
    }
}