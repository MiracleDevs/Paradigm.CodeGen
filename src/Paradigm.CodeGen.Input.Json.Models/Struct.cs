using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Struct : ObjectBase
    {
        [DataMember]
        public List<Property> Properties { get; set; }

        [DataMember]
        public List<Method> Methods { get; set; }

        [DataMember]
        public List<ObjectBase> GenericArguments { get; set; }

        public Struct()
        {
            this.Properties = new List<Property>();
            this.Methods = new List<Method>();
            this.GenericArguments = new List<ObjectBase>();
        }

        public void AddProperty(Property definition)
        {
            this.Properties?.Add(definition);
        }

        public void AddMethod(Method definition)
        {
            this.Methods?.Add(definition);
        }

        public void AddGenericArgument(ObjectBase definition)
        {
            this.GenericArguments?.Add(definition);
        }

        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}