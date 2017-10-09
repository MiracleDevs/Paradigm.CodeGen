using System;
using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations;
using Paradigm.CodeGen.Logging;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.Input.NetAssembly.Collections.Implementations
{
    public class ObjectDefinitions : IObjectDefinitions<Type>
    {
        private IServiceProvider ServiceProvider { get; }

        private Dictionary<string, ObjectDefinitionBase> Definitions { get; }

        private ILoggingService LoggingService { get; }

        public ObjectDefinitions(IServiceProvider serviceProvider, ILoggingService loggingService)
        {
            this.ServiceProvider = serviceProvider;
            this.LoggingService = loggingService;
            this.Definitions = new Dictionary<string, ObjectDefinitionBase>();
        }

        public void Add(Type key, ObjectDefinitionBase definition)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            this.Definitions.Add(definition.FullName, definition);
        }

        public bool ContainsDefinition(Type key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return this.Definitions.ContainsKey(key.GetReadableFullName()) || this.Definitions.ContainsKey(key.GetReadableName());
        }

        public ObjectDefinitionBase Get(Type key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            try
            {
                var fullName = key.GetReadableFullName();
                var name = key.GetReadableName();

                if (this.Definitions.ContainsKey(fullName))
                    return this.Definitions[fullName];

                if (this.Definitions.ContainsKey(name))
                    return this.Definitions[name];

                var provider = ObjectDefinitionFactory.GetProvider(this.ServiceProvider, key);
                var newObjectDefinition = provider.GetFromSource(key);
                this.Add(key, newObjectDefinition);
                provider.Process(newObjectDefinition, this, key);

                return newObjectDefinition;
            }
            catch (Exception e)
            {
                this.LoggingService.Error(e.Message);
                return null;
            }
        }

        public IEnumerable<ObjectDefinitionBase> Find(Func<ObjectDefinitionBase, bool> predicate = null)
        {
            return predicate != null
                ? this.Definitions.Values.Where(predicate)
                : this.Definitions.Values;
        }
    }
}