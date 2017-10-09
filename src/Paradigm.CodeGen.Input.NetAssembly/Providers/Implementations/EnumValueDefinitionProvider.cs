using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public class EnumValueDefinitionProvider : DefinitionProviderBase<EnumValueDefinition, object, Type>, IEnumValueDefinitionProvider<object, Type>
    {
        public EnumValueDefinitionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override EnumValueDefinition GetFromSource(object parameter)
        {
            return new EnumValueDefinition { Name = parameter.ToString(), Value = GetValue(parameter) };
         
        }

        public override EnumValueDefinition Process(EnumValueDefinition definition, IObjectDefinitions<Type> objectDefinitions, object parameter)
        {
            return definition;
        }

        private string GetValue(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Int64:
                    return ((long)value).ToString();
                case TypeCode.Int16:
                    return ((short)value).ToString();
                case TypeCode.UInt16:
                    return ((ushort)value).ToString();
                case TypeCode.UInt32:
                    return ((uint)value).ToString();
                case TypeCode.UInt64:
                    return ((ulong)value).ToString();
                case TypeCode.Byte:
                    return ((byte)value).ToString();
                case TypeCode.SByte:
                    return ((sbyte)value).ToString();
                default:
                    return ((int)value).ToString();
            }
        }
    }
}