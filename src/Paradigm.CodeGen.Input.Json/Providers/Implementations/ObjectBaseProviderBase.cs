using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public abstract class ObjectBaseProviderBase : DefinitionProviderBase<ObjectBase, ObjectDefinitionBase>, IObjectBaseProvider<ObjectBase, ObjectDefinitionBase>
    {
        protected ObjectBaseProviderBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ObjectDefinitionBase Process(ObjectDefinitionBase output, ObjectBase input, IInputService service)
        {
            var attributeProvider = this.Resolve<IAttributeProvider>();

            output.IsAbstract = input.IsAbstract;
            output.IsArray = input.IsArray;
            output.IsInterface = input.IsInterface;
            output.Namespace = input.Namespace;

            if (input.Attributes != null)
                output.Attributes = input.Attributes.Select(x => attributeProvider.Process(attributeProvider.Create(x), x, service)).ToList();

            if (input.ImplementedInterfaces != null)
                output.ImplementedInterfaces = input.ImplementedInterfaces.Select(x => service.GetObjectDefinitions().FirstOrDefault(o => x == o.FullName)).ToList();

            if (!string.IsNullOrWhiteSpace(input.BaseTypeName))
                output.BaseType = service.GetObjectDefinitions().FirstOrDefault(x => x.FullName == input.FullName);

            if (!string.IsNullOrWhiteSpace(input.InnerObjectName))
                output.InnerObject = service.GetObjectDefinitions().FirstOrDefault(x => x.FullName == input.InnerObjectName);

            return base.Process(output, input, service);
        }
    }
}