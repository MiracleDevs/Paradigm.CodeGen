using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Logging;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using Paradigm.CodeGen.Output.Razor.Collections.Interfaces;
using Paradigm.CodeGen.Output.Templating;
using Paradigm.CodeGen.Output.TypeMatchers;
using Paradigm.CodeGen.Output.Razor.Collections.Implementations;

namespace Paradigm.CodeGen.Output.Razor
{
    public class OutputService : IOutputService
    {
        #region Properties

        private const string NativeTypeTranslatorsParameterName = "nativeTypeTranslatorsPath";

        private IServiceProvider ServiceProvider { get; }

        private ITemplateService TemplateService { get; }

        private ILoggingService LoggingService { get; }

        private IInputService TranslationService { get; }

        private INativeTypeTranslators NativeTranslators { get; set; }

        private List<GenerationItem> GenerationItems { get; }

        private string SourcePath { get; set; }

        #endregion

        #region Constructor

        public OutputService(IServiceProvider serviceProvider, ITemplateService templateService, IInputService translationService, ILoggingService loggingService)
        {
            this.ServiceProvider = serviceProvider;
            this.TemplateService = templateService;
            this.TranslationService = translationService;
            this.LoggingService = loggingService;
            this.GenerationItems = new List<GenerationItem>();
        }

        #endregion

        #region Public Methods

        public void Generate(string fileName, OutputConfiguration configuration)
        {
            this.LoggingService.Notice("Begining Code Generation...");

            this.SourcePath = Path.GetDirectoryName(fileName);
            this.LoadNativeTranslators(configuration);
            this.LoadTemplates(configuration);
            this.ProcessOutputFiles(configuration);

            if (configuration.Summary == null)
                return;

            this.GenerateFinalSummary(fileName, configuration);
        }

        #endregion

        #region Private Methods

        private void ProcessOutputFiles(OutputConfiguration configuration)
        {
            foreach (var outputConfiguration in configuration.OutputFiles)
            {
                try
                {
                    this.ProcessCodeGeneration(outputConfiguration);
                }
                catch (Exception e)
                {
                    this.LoggingService.Error($"Problems found in output configuration file [{outputConfiguration.Name}]':{Environment.NewLine}{e.Message}");
                }
            }
        }

        private void ProcessCodeGeneration(OutputFileConfiguration configuration)
        {
            this.LoggingService.Write($" - Starting output configuration file [{configuration.Name}]: ");
            var template = TemplateCache.Instance.Get(configuration.TemplatePath);
            var generationItems = new List<GenerationItem>();

            foreach (var objectDefiniton in this.TranslationService.GetObjectDefinitions().Where(x => IsMatch(x, configuration)))
            {
                try
                {
                    generationItems.Add(this.GenerateFile(configuration, objectDefiniton, template));
                }
                catch (Exception e)
                {
                    this.LoggingService.Error($"Problems found in object definition '{objectDefiniton.Name}':{Environment.NewLine}{e.Message}");
                }
            }

            this.LoggingService.WriteLine($"{generationItems.Count} files generated.");

            if (configuration.Summary != null)
                this.GenerateOutputFileSummary(configuration, generationItems);

            this.GenerationItems.AddRange(generationItems);
        }

        private void GenerateOutputFileSummary(OutputFileConfiguration configuration, List<GenerationItem> generationItems)
        {
            try
            {
                this.LoggingService.Write($" - Generating Summary for [{configuration.Name}]: ");
                this.GenerateSummaryFile(configuration.Summary, generationItems);
                this.LoggingService.WriteLine("Summary Generated.");
            }
            catch (Exception ex)
            {
                this.LoggingService.Error(ex.Message);
            }
        }

        private void GenerateFinalSummary(string fileName, OutputConfiguration configuration)
        {
            try
            {
                this.LoggingService.Write($" - Generating Summary for [{Path.GetFileName(fileName)}]: ");
                this.GenerateSummaryFile(configuration.Summary, this.GenerationItems);
                this.LoggingService.WriteLine("Summary Generated.");
            }
            catch (Exception ex)
            {
                this.LoggingService.Error(ex.Message);
            }
        }

        private GenerationItem GenerateFile(OutputFileConfiguration configuration, ObjectDefinitionBase definition, ITemplate template)
        {
            var outputFilename = ResolveOutputName(configuration, definition);
            var outputDirectory = Path.GetFullPath($"{this.SourcePath}/{configuration.OutputPath}");
            var generationItem = new GenerationItem(configuration, new GenerationItemModel(configuration, definition, this.TranslationService.GetObjectDefinitions(), this.NativeTranslators), Path.GetFullPath($"{outputDirectory}/{outputFilename}"));

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            File.WriteAllText(generationItem.OutputFileName, this.TemplateService.Execute(template, generationItem.Model, this.LoggingService));
            return generationItem;
        }

        private void GenerateSummaryFile(SummaryFileConfiguration configuration, List<GenerationItem> generationItems)
        {
            var template = TemplateCache.Instance.Get(configuration.TemplatePath);
            var outputFile = Path.GetFullPath($"{this.SourcePath}/{configuration.OutputFileName}");
            var outputDirectory = Path.GetDirectoryName(outputFile);
            var summaryModel = new SummaryModel(configuration, generationItems);

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            File.WriteAllText(outputFile, this.TemplateService.Execute(template, summaryModel, this.LoggingService));
        }

        private static string ResolveOutputName(OutputFileConfiguration outputConfiguration, ObjectDefinitionBase definition)
        {
            return outputConfiguration.NamingRules.Aggregate(definition.Name, (current, rule) => NamingRuleFactory.Create(rule.Name).Execute(current, rule));
        }

        private static bool IsMatch(ObjectDefinitionBase objectDefinition, OutputFileConfiguration outputConfiguration)
        {
            return outputConfiguration.TypeMatchers.Any() &&
                   outputConfiguration.TypeMatchers.Aggregate(true, (current, typeMatcherConfiguration) => current & TypeMatcherFactory.Create(typeMatcherConfiguration.Name).Match(typeMatcherConfiguration, objectDefinition));
        }

        private void LoadNativeTranslators(OutputConfiguration configuration)
        {
            var nativeTranslatorsPath = configuration.Parameters.FirstOrDefault(x => x.Name == NativeTypeTranslatorsParameterName)?.Value;

            this.NativeTranslators = !string.IsNullOrWhiteSpace(nativeTranslatorsPath)
                ? NativeTypeTranslators.FromJson(Path.GetFullPath($"{this.SourcePath}/{nativeTranslatorsPath}"))
                : new NativeTypeTranslators();
        }

        private void LoadTemplates(OutputConfiguration configuration)
        {
            this.LoggingService.Notice("Registering Templates:");

            foreach (var outputConfiguration in configuration.OutputFiles)
            {
                var fileName = outputConfiguration.TemplatePath;
                this.AddTemplate(fileName);

                if (outputConfiguration.Summary != null)
                {
                    this.AddTemplate(outputConfiguration.Summary.TemplatePath);
                }
            }

            if (configuration.Summary != null)
            {
                this.AddTemplate(configuration.Summary.TemplatePath);
            }
        }

        private void AddTemplate(string fileName)
        {
            if (TemplateCache.Instance.IsRegistered(fileName))
                return;

            this.LoggingService.WriteLine($" - Template [{Path.GetFileName(fileName)}]");
            var template = this.ServiceProvider.GetService<ITemplate>();
            template.Open(Path.GetFullPath($"{this.SourcePath}/{fileName}"));

            TemplateCache.Instance.Register(fileName, template);
        }

        #endregion
    }
}