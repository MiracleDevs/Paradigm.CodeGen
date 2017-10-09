using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Attribute
    {     
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<AttributeParameter> Parameters { get; set; }

        public Attribute()
        {
            this.Parameters = new List<AttributeParameter>();
        }

        public override string ToString()
        {
            return $"attribute {this.Name}";
        }

        public void AddParameter(AttributeParameter parameter)
        {
            this.Parameters.Add(parameter);
        }
    }
}