using System;
using System.Collections.Generic;
using Paradigm.CodeGen.Input.Models.Translations;

namespace Paradigm.CodeGen.Output.Razor.Collections.Interfaces
{
    public interface INativeTypeTranslators
    {
        void Add(NativeTypeTranslator translator);

        bool ContainsTranslator(string key);

        NativeTypeTranslator Get(string key);

        IEnumerable<NativeTypeTranslator> Find(Func<NativeTypeTranslator, bool> predicate = null);
    }
}