using System;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public abstract class MemberBaseProviderBase<TInput, TOutput> : DefinitionProviderBase<TInput, TOutput>, IMemeberBaseProvider<TInput, TOutput>
        where TInput : MemberBase
        where TOutput : MemberDefinitionBase, new()
    {
        protected MemberBaseProviderBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override TOutput Create(TInput input)
        {
            return new TOutput
            {
                Name = input.Name
            };
        }

        public override TOutput Process(TOutput output, TInput input, IInputService service)
        {
            var attributeProvider = this.Resolve<IAttributeProvider>();

            if (input.Attributes != null)
                output.Attributes = input.Attributes.Select(x => attributeProvider.Process(attributeProvider.Create(x), x, service)).ToList();

            if (!string.IsNullOrWhiteSpace(input.TypeName))
                output.Type = service.GetObjectDefinitions().FirstOrDefault(x => x.FullName == input.TypeName);

            return base.Process(output, input, service);
        }
    }
}