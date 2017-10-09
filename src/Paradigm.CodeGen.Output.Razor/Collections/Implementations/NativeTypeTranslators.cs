using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Translations;
using Paradigm.CodeGen.Output.Razor.Collections.Interfaces;
using Newtonsoft.Json;

namespace Paradigm.CodeGen.Output.Razor.Collections.Implementations
{
    public class NativeTypeTranslators : INativeTypeTranslators
    {
        private Dictionary<string, NativeTypeTranslator> TypeTranslators { get; }

        public NativeTypeTranslators()
        {
            this.TypeTranslators = new Dictionary<string, NativeTypeTranslator>();
        }

        public static NativeTypeTranslators FromJson(string fileName)
        {
            var translators = JsonConvert.DeserializeObject<List<NativeTypeTranslator>>(File.ReadAllText(fileName));

            if (translators == null)
                throw new Exception("Couldn't deserialize type translators json file.");

            var typeTranslations = new NativeTypeTranslators();
            translators.ForEach(x => typeTranslations.TypeTranslators.Add(x.Name, x));

            return typeTranslations;
        }

        public void Add(NativeTypeTranslator translator)
        {
            if (translator == null)
                throw new ArgumentNullException(nameof(translator));

            this.TypeTranslators.Add(translator.Name, translator);
        }

        public bool ContainsTranslator(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return this.TypeTranslators.ContainsKey(key);
        }

        public NativeTypeTranslator Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var translator = this.ContainsTranslator(key)
                ? this.TypeTranslators[key] 
                : null;

            return translator; 
        }

        public IEnumerable<NativeTypeTranslator> Find(Func<NativeTypeTranslator, bool> predicate = null)
        {
            return predicate != null 
                ? this.TypeTranslators.Values.Where(predicate) 
                : this.TypeTranslators.Values;
        }
    }
}