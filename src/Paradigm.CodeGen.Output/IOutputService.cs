using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output
{
    public interface IOutputService
    {
        void Generate(string fileName, OutputConfiguration configuration, int maxParallelism);
    }
}