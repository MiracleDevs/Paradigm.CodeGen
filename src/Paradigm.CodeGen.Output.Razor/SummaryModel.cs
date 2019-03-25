using System.Collections.Generic;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.Output.Razor
{
    public class SummaryModel
    {
        public SummaryFileConfiguration Configuration { get; set; }

        public List<GenerationItem> GenerationItems { get; set; }

        public SummaryModel(SummaryFileConfiguration configuration, List<GenerationItem> generationItems)
        {
            this.Configuration = configuration;
            this.GenerationItems = generationItems;
        }

        public override string ToString()
        {
            return $"Output File: '{Configuration.OutputFileName}'";
        }
    }
}