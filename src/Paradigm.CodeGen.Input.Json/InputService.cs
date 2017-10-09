using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Implementations;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.CodeGen.Input.Models.Definitions;
using Newtonsoft.Json;

namespace Paradigm.CodeGen.Input.Json
{
    public class InputService : IInputService
    {
        private const string JsonFileParameterName = "jsonFiles";

        private const string JsonPathParameterName = "jsonPaths";

        private const string JsonFileExtensionParameterName = "jsonFileExtension";

        private const string JsonPathIncludeSubFoldersParameterName = "jsonPathIncludeSubFolders";

        private IServiceProvider ServiceProvider { get; }

        private List<ObjectDefinitionBase> Definitions { get; }

        public InputService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.Definitions = new List<ObjectDefinitionBase>();
        }

        public void Process(string fileName, InputConfiguration configuration)
        {
            var files = this.GetFileNames(fileName, configuration);
            var objectTuples = new List<Tuple<ObjectBase, ObjectDefinitionBase>>();

            foreach (var file in files)
            {
                var objectContainers = JsonConvert.DeserializeObject<List<ObjectContainer>>(File.ReadAllText(file));

                foreach (var objectContainer in objectContainers)
                {
                    var objectDefinition = ObjectProviderFactory.GetProvider(this.ServiceProvider, objectContainer.Object).Create(objectContainer.Object);
                    this.Definitions.Add(objectDefinition);
                    objectTuples.Add(new Tuple<ObjectBase, ObjectDefinitionBase>(objectContainer.Object, objectDefinition));
                }
            }

            foreach (var objectTuple in objectTuples)
            {
                ObjectProviderFactory.GetProvider(this.ServiceProvider, objectTuple.Item1).Process(objectTuple.Item2, objectTuple.Item1, this);
            }
        }

        public IEnumerable<ObjectDefinitionBase> GetObjectDefinitions()
        {
            return this.Definitions;
        }

        private IEnumerable<string> GetFileNames(string configurationFileName, InputConfiguration configuration)
        {
            var fileList = new List<string>();
            var directory = Path.GetDirectoryName(configurationFileName);

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            IReadOnlyDictionary<string, string> parameters = configuration.Parameters.ToDictionary(x => x.Name, x => x.Value);
        
            if (parameters.ContainsKey(JsonPathParameterName))
            {
                var pathArray = parameters[JsonPathParameterName].Split(',');

                foreach (var path in pathArray)
                {
                    var searchPattern = "*." + (parameters.ContainsKey(JsonFileExtensionParameterName) ? parameters[JsonFileExtensionParameterName] ?? string.Empty : "json");
                    var includeSubFolder = parameters.ContainsKey(JsonPathIncludeSubFoldersParameterName) && Convert.ToBoolean(parameters[JsonPathIncludeSubFoldersParameterName] ?? "true");
                    var lookupPath = Path.GetFullPath($"{directory}/{path}");

                    if (!Directory.Exists(lookupPath))
                        throw new Exception($"The path '{lookupPath}' specified to look up for the json files does not exist.");

                    fileList.AddRange(Directory.EnumerateFiles(lookupPath, searchPattern, includeSubFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                }
            }

            if (parameters.ContainsKey(JsonFileParameterName))
            {
                var fileArray = parameters[JsonFileParameterName].Split(',');
                
                foreach(var file in fileArray)
                {
                    var fileName = Path.GetFullPath($"{directory}/{file}");

                    if (!File.Exists(fileName))
                        throw new Exception($"The filename '{fileName}' of one of the json configuration files does not exist.");

                    fileList.Add(fileName);
                }
            }

            return fileList;
        }
    }
}