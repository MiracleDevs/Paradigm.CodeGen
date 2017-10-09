using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public abstract class StructProviderBase : ObjectBaseProviderBase
    {
        protected StructProviderBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase output, ObjectBase input, IInputService service)
        {
            var methodProvider = this.Resolve<IMethodProvider>();
            var propertyProvider = this.Resolve<IPropertyProvider>();
            var structOutput = output as StructDefinition;
            var structInput = input as Struct;

            if (structOutput == null || structInput == null)
                return output;

            if (structInput.Methods != null)
                structOutput.Methods = structInput.Methods.Select(x => methodProvider.Process(methodProvider.Create(x), x, service)).ToList();

            if (structInput.Properties != null)
                structOutput.Properties = structInput.Properties.Select(x => propertyProvider.Process(propertyProvider.Create(x), x, service)).ToList();

            if (structInput.GenericArguments != null)
                structOutput.GenericArguments = structInput.GenericArguments.Select(x => service.GetObjectDefinitions().FirstOrDefault(o => o.FullName == x.FullName)).ToList();

            return base.Process(output, input, service);
        }
    }
}