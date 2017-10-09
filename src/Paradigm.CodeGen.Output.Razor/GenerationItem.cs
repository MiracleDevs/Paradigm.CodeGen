using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.Razor
{
    public class GenerationItem
    {
        public OutputFileConfiguration Configuration { get; set; }

        public GenerationItemModel Model { get; set; }

        public string OutputFileName { get; set; }

        public GenerationItem(OutputFileConfiguration configuration, GenerationItemModel model, string fileName)
        {
            this.Configuration = configuration;
            this.Model = model;
            this.OutputFileName = fileName;
        }
    }
}