using Paradigm.CodeGen.Input.Json.Providers.Implementations;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Input.Json
{
    public class InputPlugin : IInputPlugin
    {
        public void RegisterPlugin(InputConfiguration configuration, IDependencyBuilder builder)
        {
            builder.Register<IAttributeProvider, AttributeProvider>();
            builder.Register<IAttributeParameterProvider, AttributeParameterProvider>();
            builder.Register<IClassProvider, ClassProvider>();
            builder.Register<IEnumProvider, EnumProvider>();
            builder.Register<IEnumValueProvider, EnumValueProvider>();
            builder.Register<IMethodProvider, MethodProvider>();
            builder.Register<IParameterProvider, ParameterProvider>();
            builder.Register<IPropertyProvider, PropertyProvider>();
            builder.Register<IStructProvider, StructProvider>();
            builder.RegisterScoped<IInputService, InputService>();
        }
    }
}