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
            return this.Templates.ContainsKey(this.GetFullFileName(fileName));
        }

        public void Register(string fileName, ITemplate template)
        {
            fileName = this.GetFullFileName(fileName);
            this.Templates.Add(fileName, template);
            template.Open(fileName);

        }

        public ITemplate Get(string fileName)
        {
            fileName = this.GetFullFileName(fileName);

            if (!IsRegistered(fileName))
                throw new Exception($"Template '{this.GetFullFileName(fileName)}' is not registed.");

            return this.Templates[fileName];
        }

        private string GetFullFileName(string fileName)
        {
            return Path.IsPathRooted(fileName) ? fileName : Path.GetFullPath(fileName);
        }

        #endregion

    }
}