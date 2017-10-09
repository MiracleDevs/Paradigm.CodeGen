﻿using System;
using System.Collections.Generic;
using Paradigm.CodeGen.Logging;
using Paradigm.Core.DependencyInjection;
using System.IO;
using System.Linq;
using Paradigm.Core.Assemblies;
using Newtonsoft.Json;
using System.Runtime.Loader;
using Paradigm.CodeGen.Input;
using Paradigm.CodeGen.Output;
using Microsoft.Extensions.CommandLineUtils;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.UI.Console
{
    internal class Program
    {
        private static ILoggingService LoggingService { get; set; }

        private static AssemblyLoader AssemblyLoader { get; set; }

        public static void Main(string[] args)
        {
            LoggingService = new ConsoleLoggingService();
            var started = DateTime.Now;

            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice("Miracle Devs");
            LoggingService.Notice("Code Generation Tool");
            LoggingService.Notice("");
            LoggingService.Notice($"Started at: {started}");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

            ConfigureAssemblyLoader();
            ParseCommandLine(args);

            var ended = DateTime.Now;
            LoggingService.WriteLine("");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice($"Ended at: {ended}");
            LoggingService.Notice($"Elapsed: { new TimeSpan(ended.Subtract(started).Ticks).TotalSeconds } sec");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

#if DEBUG
            System.Console.ReadKey();
#endif
        }

        private static void ConfigureAssemblyLoader()
        {
            var location = typeof(Program).GetAssembly().Location;
            var path = Path.GetDirectoryName(location);

            AssemblyLoader = new AssemblyLoader(new[] { path }, true);
            AssemblyLoadContext.Default.Resolving += (alc, an) =>
            {
                LoggingService.WriteLine($"Requesting assembly [{an.Name}]");
                return AssemblyLoader.ResolveAssembly(an, alc, AssemblyLoader.OptionalDirectories, AssemblyLoader.NugetLookUp);
            };
        }

        private static void ParseCommandLine(params string[] args)
        {
            try
            {
                var commandLineApplication = new CommandLineApplication(false);
                var fileNames = commandLineApplication.Option("-f | --filename <filename>", "Indicates the path of one or more output configuration files.", CommandOptionType.MultipleValue);
                var directories = commandLineApplication.Option("-d | --directory <directory>", "Indicates one or more directory path in which all the output configuration files will be used to generate code.", CommandOptionType.MultipleValue);
                var topDirectoryOnly = commandLineApplication.Option("-t | --topdirectory", "If directories were provided, indicates if the system should check only on the top directory.", CommandOptionType.NoValue);
                var extension = commandLineApplication.Option("-e | --extension <extension>", "Indicates the extension of configuration files when searching inside directories.", CommandOptionType.SingleValue);
                var output = commandLineApplication.Option("-o | --override <outputFile>:<typeName>", "Allows to override the configuration file and choose an Output File Configuration for a given Type Name. Take in account that any other configuration won't be executed if you override the configuration files.", CommandOptionType.MultipleValue);

                commandLineApplication.HelpOption("-? | -h | --help");
                commandLineApplication.OnExecute(() =>
                {
                    GetConfigurationFiles(fileNames.Values, directories.Values, topDirectoryOnly.HasValue(), extension.Value()).ToList().ForEach(x => ProcessConfigurationFile(x, GetOutputFileOverrides(output.Values)));
                    return 0;
                });

                commandLineApplication.Execute(args);
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex.Message);
            }
        }

        private static void ProcessConfigurationFile(string configurationFileName, List<OutputFileOverride> outputFileOverrides)
        {
            LoggingService.Notice($"Starting to read configuration file [{Path.GetFileName(configurationFileName)}]");

            // read the configuration file.
            var configuration = GetConfigurationFile(configurationFileName);

            if (configuration == null)
                return;

            // overrides the outfile typematching adding "NameIn" type matcher,
            // and adds all the typed names to the output file configuration for this run.
            OverrideConfigurationFiles(outputFileOverrides, configuration);

            LoggingService.WriteLine($"Input  Type:\t{configuration.Input.InputType}");
            LoggingService.WriteLine($"Output Type:\t{configuration.Output.OutputType}");

            // inject dependencies and start input and ouput plugins.
            var builder = DependencyBuilderFactory.Create(DependencyLibrary.Microsoft);

            builder.RegisterInstance<ILoggingService, ConsoleLoggingService>((ConsoleLoggingService)LoggingService);
            builder.RegisterInstance(AssemblyLoader);

            InputPluginLoader.Load(builder, configuration.Input, AssemblyLoadContext.Default);
            OutputPluginLoader.Load(builder, configuration.Output, AssemblyLoadContext.Default);

            var container = builder.Build();

            // instantiate input and output plugin.
            var inputService = container.Resolve<IInputService>();
            var outputService = container.Resolve<IOutputService>();

            // run input and output logic.
            inputService.Process(configurationFileName, configuration.Input);
            outputService.Generate(configurationFileName, configuration.Output);
        }

        private static CodeGeneratorConfiguration GetConfigurationFile(string configurationFileName)
        {
            try
            {
                var configuration = JsonConvert.DeserializeObject<CodeGeneratorConfiguration>(File.ReadAllText(configurationFileName));
                return configuration;
            }
            catch 
            {
                LoggingService.Error($"The configuration file couldn't be opened [{configurationFileName}]");
                return null;
            }
        }

        private static IEnumerable<string> GetConfigurationFiles(IEnumerable<string> fileNames, IEnumerable<string> directories, bool topDirectoryOnly, string extension)
        {
            var path = Directory.GetCurrentDirectory();
            var files = new List<string>();

            LoggingService.Notice($"Discovering Files");
            LoggingService.WriteLine($"    Directory:       [{path}]");

            foreach (var fileName in fileNames)
            {
                var fullFileName = Path.GetFullPath($"{path}/{fileName}");

                if (File.Exists(fullFileName))
                    files.Add(fullFileName);
                else
                    LoggingService.Error($"File not found [{fullFileName}]");
            }

            foreach (var directory in directories)
            {
                var fullDirectoryPath = Path.GetFullPath($"{path}/{directory}");
                LoggingService.WriteLine($"     Processing Directory [{fullDirectoryPath}]");

                if (Directory.Exists(fullDirectoryPath))
                    files.AddRange(Directory.EnumerateFiles(fullDirectoryPath, $"*.{extension ?? "json"}", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));
                else
                    LoggingService.Error($"Directory not found [{fullDirectoryPath}]");
            }

            LoggingService.WriteLine(string.Empty);
            LoggingService.Notice("Individual Files:");
            files.ForEach(x => LoggingService.WriteLine($"    {x}"));
            LoggingService.WriteLine(string.Empty);

            return files;

        }

        private static List<OutputFileOverride> GetOutputFileOverrides(IEnumerable<string> configuration)
        {
            return configuration.Select(x => new OutputFileOverride(x)).ToList();
        }

        private static void OverrideConfigurationFiles(IReadOnlyCollection<OutputFileOverride> outputFileOverrides, CodeGeneratorConfiguration configuration)
        {
            if (outputFileOverrides == null || !outputFileOverrides.Any())
                return;

            foreach (var outputFile in configuration.Output.OutputFiles.ToList())
            {
                var overrides = outputFileOverrides.Where(x => x.OutputFileConfigurationName == outputFile.Name).ToList();

                if (overrides != null && overrides.Any())
                {
                    outputFile.TypeMatchers = new List<TypeMatcherConfiguration>(new[] { new TypeMatcherConfiguration { Name = "NameIn", Parameters = overrides.Select(x => x.TypeName).ToArray() } });
                    LoggingService.WriteLine($"- Output File Configuration [{outputFile.Name}] has been overriden for types [{string.Join(", ", overrides.Select(x => x.TypeName))}].");
                }
                else
                {
                    configuration.Output.OutputFiles.Remove(outputFile);
                    LoggingService.WriteLine($"- Removing Output File Configuration [{outputFile.Name}] due to overriden functions.");
                }
            }
        }
    }
}