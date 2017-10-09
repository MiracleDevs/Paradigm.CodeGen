using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;
using Paradigm.Core.Extensions;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class ClassDefinitionProvider : StructDefinitionProvider, IClassDefinitionProvider<Type, Type>
    {
        public ClassDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase GetFromSource(Type parameter)
        {
            return new ClassDefinition
            {
                Name = parameter.GetReadableName(),
                FullName =  parameter.GetReadableFullName()
            };
        }
    }
}