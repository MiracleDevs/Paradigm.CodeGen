using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Paradigm.CodeGen.Input
{
    /// <inheritdoc />
    /// <summary>
    /// Extends the default <see cref="T:System.Runtime.Loader.AssemblyLoadContext" /> providing custom folders to search the assemblies being loaded.
    /// </summary>
    /// <remarks>
    /// When dynamically loading assemblies, this class provides the means to search in all the specified assemblies.
    /// By default, if the framework can not find the assembly, this class will come in and try to find the assembly
    /// in one of the provided folders.
    /// </remarks>
    /// <seealso cref="T:System.Runtime.Loader.AssemblyLoadContext" />
    public class AssemblyLoader : AssemblyLoadContext
    {
        /// <summary>
        /// Gets the optional directories.
        /// </summary>
        public List<string> OptionalDirectories { get; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Paradigm.CodeGen.Input.AssemblyLoader" /> class.
        /// </summary>
        /// <param name="optionalDirectories">The optional directories.</param>
        public AssemblyLoader(IEnumerable<string> optionalDirectories = null)
        {
            this.OptionalDirectories = optionalDirectories?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Adds an optional directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public void AddOptionalDirectory(string directoryPath)
        {
            if (this.OptionalDirectories.Any(x => x == directoryPath))
                return;

            this.OptionalDirectories.Add(directoryPath);
        }

        /// <inheritdoc />
        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return ResolveAssembly(assemblyName, this, this.OptionalDirectories);
        }

        /// <summary>
        /// Resolves the assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="assemblyLoadContext">The assembly load context.</param>
        /// <param name="optionalDirectories">The optional directories.</param>
        /// <param name="nugetLookUp">if set to <c>true</c> the system will look inside the global nuget directory.</param>
        /// <returns></returns>
        public static Assembly ResolveAssembly(AssemblyName assemblyName, AssemblyLoadContext assemblyLoadContext, IEnumerable<string> optionalDirectories, bool nugetLookUp = false)
        {
            var possibleAssemblies = new List<string>();
            var assemblyFileName = $"{assemblyName.Name}.dll";

            foreach (var directory in optionalDirectories)
            {
                if (Directory.Exists(directory))
                {
                    possibleAssemblies.AddRange(Directory.EnumerateFiles(directory, assemblyFileName, SearchOption.AllDirectories));
                }
            }

            foreach (var assemblyPath in possibleAssemblies)
            {
                try
                {
                     return assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
                }
                catch
                {
                    // ignored
                }
            }

            return null;
        }
    }
}
