using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.Models.Translations;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.Razor.Collections.Interfaces;

namespace Paradigm.CodeGen.Output.Razor
{
    public class GenerationItemModel
    {
        public OutputFileConfiguration Configuration { get; set; }

        public ObjectDefinitionBase Definition { get; set; }

        public IEnumerable<ObjectDefinitionBase> Definitions { get; set; }

        public INativeTypeTranslators TypeTranslators { get; set; }

        public GenerationItemModel(OutputFileConfiguration configuration, ObjectDefinitionBase definition, IEnumerable<ObjectDefinitionBase> definitions, INativeTypeTranslators typeTranslators)
        {
            this.Configuration = configuration;
            this.Definition = definition;
            this.Definitions = definitions;
            this.TypeTranslators = typeTranslators;
        }

        public NativeTypeTranslator GetTranslator(ObjectDefinitionBase definition)
        {
            return this.TypeTranslators.Find(x => x.Name == definition.FullName || x.Name == definition.Name).FirstOrDefault();
        }
    }
}