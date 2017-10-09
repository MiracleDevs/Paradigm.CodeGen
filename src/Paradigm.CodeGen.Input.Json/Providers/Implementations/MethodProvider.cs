using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class MethodProvider: DefinitionProviderBase<Method, MethodDefinition>, IMethodProvider
    {
        public MethodProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override MethodDefinition Create(Method input)
        {
            return new MethodDefinition();
        }

        public override MethodDefinition Process(MethodDefinition output, Method input, IInputService service)
        {
            var attributeProvider = this.Resolve<IAttributeProvider>();
            var parameterProvider = this.Resolve<IParameterProvider>();

            if (!string.IsNullOrWhiteSpace(input.ReturnTypeName))
                output.ReturnType = service.GetObjectDefinitions().FirstOrDefault(x => x.FullName == input.ReturnTypeName);

            if (input.Attributes != null)
                output.Attributes = input.Attributes.Select(x => attributeProvider.Process(attributeProvider.Create(x), x, service)).ToList();

            if (input.Parameters != null)
                output.Parameters = input.Parameters.Select(x => parameterProvider.Process(parameterProvider.Create(x), x, service)).ToList();

            return base.Process(output, input, service);
        }
    }
}