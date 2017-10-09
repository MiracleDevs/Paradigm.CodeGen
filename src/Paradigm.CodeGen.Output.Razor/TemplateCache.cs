using System;
using System.Collections.Generic;
using System.IO;
using Paradigm.CodeGen.Output.Templating;

namespace Paradigm.CodeGen.Output.Razor
{
    internal class TemplateCache
    {
        #region Singleton

        private static readonly Lazy<TemplateCache> InternalInstance = new Lazy<TemplateCache>(() => new TemplateCache(), true);

        public static TemplateCache Instance => InternalInstance.Value;

        #endregion

        #region Properties
        
        private Dictionary<string, ITemplate> Templates { get; }

        #endregion

        #region Constructor

        private TemplateCache()
        {
            this.Templates = new Dictionary<string, ITemplate>();
        }

        #endregion

        #region Public Methods

        public bool IsRegistered(string fileName)
        {
            return this.Templates.ContainsKey(fileName);
        }

        public void Register(string fileName, ITemplate template)
        {
            this.Templates.Add(fileName, template);
        }

        public ITemplate Get(string fileName)
        {
            if (!IsRegistered(fileName))
                throw new Exception($"Template '{Path.GetFileName(fileName)}' is not registed.");

            return this.Templates[fileName];
        }

        #endregion

    }
}