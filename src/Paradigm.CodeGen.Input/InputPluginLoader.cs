using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Input
{
    public static class InputPluginLoader
    {
        public static IInputPlugin Load(IDependencyBuilder builder, InputConfiguration configuration, AssemblyLoadContext assemblyLoader)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.InputType))
                throw new Exception("Can not instantiate a translation plugin without a plugin name.");

            var assemblyName = $"{typeof(InputPluginLoader).Namespace}.{configuration.InputType}";
            var assembly = assemblyLoader.LoadFromAssemblyName(new AssemblyName(assemblyName));

            var type = assembly.GetExportedTypes().FirstOrDefault(x => typeof(IInputPlugin).GetTypeInfo().IsAssignableFrom(x));

            if (type == null)
                throw new Exception($"Can not find a class implementing '{nameof(IInputPlugin)}' in assembly '{assembly.FullName}'.");

            var plugin = Activator.CreateInstance(type) as IInputPlugin;

            if (plugin == null)
                throw new Exception($"Couldn't instantiate '{type.Name}' plugin.");

            plugin.RegisterPlugin(configuration, builder);

            return plugin;
        }
    }
}