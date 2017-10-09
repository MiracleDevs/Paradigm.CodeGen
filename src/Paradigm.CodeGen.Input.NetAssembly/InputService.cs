using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Logging;
using Paradigm.Core.Assemblies;

namespace Paradigm.CodeGen.Input.NetAssembly
{
    public class InputService : IInputService
    {
        private const string AssemblyPathParameterName = "assemblyPath";

        private const string AssemblyEntryPoint = "entryPoint";

        public IServiceProvider ServiceProvider { get; }

        public IObjectDefinitions<Type> ObjectDefinitions { get; }

        public ILoggingService LoggingService { get; }

        public InputService(IServiceProvider serviceProvider, IObjectDefinitions<Type> objectDefinitions, ILoggingService loggingService)
        {
            this.ServiceProvider = serviceProvider;
            this.ObjectDefinitions = objectDefinitions;
            this.LoggingService = loggingService;
        }

        public void Process(string fileName, InputConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            IReadOnlyDictionary<string, string> parameters = configuration.Parameters.ToDictionary(x => x.Name, x => x.Value);

            if (!parameters.ContainsKey(AssemblyPathParameterName))
                throw new Exception($".NET Assembly Input Service requires a parameter called '{AssemblyPathParameterName}' containing a .NET Assembly path.");

            var directory = Path.GetDirectoryName(fileName);
            var assemblyPath = parameters[AssemblyPathParameterName];
            var path = Path.GetFullPath($"{directory}/{assemblyPath}");
         
            if (!File.Exists(path))
                throw new Exception($"The path '{path}' specified to look up for the assembly does not exist.");

            var assemblyLoader = this.ServiceProvider.GetService<AssemblyLoader>();
            assemblyLoader.AddOptionalDirectory(Path.GetDirectoryName(path));
            var assembly = assemblyLoader.LoadFromAssemblyPath(path);

            if (assembly == null)
                throw new Exception("The specified assembly couldn't be loaded.");

            if (parameters.ContainsKey(AssemblyEntryPoint))
            {
                var typeName = parameters[AssemblyEntryPoint];
                var type = assembly.GetType(typeName);

                if (type == null)
                    throw new Exception($"The specified entry point type '{typeName}' couldn't be loaded.");

                this.ProcessType(type);
            }
            else
                this.ProcessAllTypes(assembly);
        }

        public IEnumerable<ObjectDefinitionBase> GetObjectDefinitions()
        {
            return this.ObjectDefinitions.Find();
        }

        private void ProcessType(Type type)
        {
            try
            {
                this.LoggingService.Notice($"Starting to process Type: {type.FullName}");
                this.ObjectDefinitions.Get(type);
            }
            catch (Exception ex)
            {
                this.LoggingService.Error(ex.Message);
            }
        }

        private void ProcessAllTypes(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();

            this.LoggingService.Notice("Starting to process exported types:");

            foreach (var type in types)
            {
                try
                {
                    this.LoggingService.WriteLine($"     - Processing Type: {type.FullName}");
                    this.ObjectDefinitions.Get(type);
                }
                catch (Exception ex)
                {
                    this.LoggingService.Error(ex.Message);
                }
            }
        }
    }
}