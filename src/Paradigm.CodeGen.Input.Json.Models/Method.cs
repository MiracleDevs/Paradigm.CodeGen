using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Method
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ReturnTypeName { get; set; }

        [DataMember]
        public List<Parameter> Parameters { get; set; }

        [DataMember]
        public List<Attribute> Attributes { get; set; }

        public Method()
        {
            this.Parameters = new List<Parameter>();
            this.Attributes = new List<Attribute>();
        }

        public void AddAttribute(Attribute definition)
        {
            this.Attributes.Add(definition);
        }

        public void AddParameter(Parameter definition)
        {
            this.Parameters.Add(definition);
        }

        public override string ToString()
        {
            return $"method {this.ReturnTypeName} {this.Name}()";
        }
    }
}