using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class StructDefinition : ObjectDefinitionBase
    {
        [DataMember]
        public List<PropertyDefinition> Properties { get; set; }

        [DataMember]
        public List<MethodDefinition> Methods { get; set; }

        [DataMember]
        public List<ObjectDefinitionBase> GenericArguments { get; set; }

        public StructDefinition()
        {
            this.Properties = new List<PropertyDefinition>();
            this.Methods = new List<MethodDefinition>();
            this.GenericArguments = new List<ObjectDefinitionBase>();
        }

        public void AddProperty(PropertyDefinition definition)
        {
            this.Properties?.Add(definition);
        }

        public void AddMethod(MethodDefinition definition)
        {
            this.Methods?.Add(definition);
        }

        public void AddGenericArgument(ObjectDefinitionBase definition)
        {
            this.GenericArguments?.Add(definition);
        }

        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}