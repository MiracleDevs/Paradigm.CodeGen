using System.Threading.Tasks;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output
{
    public interface IOutputService
    {
        Task GenerateAsync(string fileName, OutputConfiguration configuration);
    }
}