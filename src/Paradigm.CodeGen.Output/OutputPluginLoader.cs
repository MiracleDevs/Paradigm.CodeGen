using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Output
{
    public static class OutputPluginLoader
    {
        public static IOutputPlugin Load(IDependencyBuilder builder, OutputConfiguration configuration, AssemblyLoadContext assemblyLoader)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.OutputType))
                throw new Exception("Can not instantiate an output plugin without an template engine name. Try a value like 'Razor'.");

            var assemblyName = $"{typeof(OutputPluginLoader).Namespace}.{configuration.OutputType}";
            var assembly = assemblyLoader.LoadFromAssemblyName(new AssemblyName(assemblyName));

            var type = assembly.GetExportedTypes().FirstOrDefault(x => typeof(IOutputPlugin).GetTypeInfo().IsAssignableFrom(x));

            if (type == null)
                throw new Exception($"Can not find a class implementing '{nameof(IOutputPlugin)}' in assembly '{assembly.FullName}'.");

            var plugin = Activator.CreateInstance(type) as IOutputPlugin;

            if (plugin == null)
                throw new Exception($"Couldn't instantiate '{type.Name}' plugin.");

            plugin.RegisterPlugin(configuration, builder);

            return plugin;
        }
    }
}