using System;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public class PropertyProvider : MemberBaseProviderBase<Property, PropertyDefinition>, IPropertyProvider
    {
        public PropertyProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}