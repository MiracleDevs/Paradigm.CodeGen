using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class AttributeDefinition
    {     
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<AttributeParameterDefinition> Parameters { get; set; }

        public AttributeDefinition()
        {
            this.Parameters = new List<AttributeParameterDefinition>();
        }

        public override string ToString()
        {
            return $"attribute {this.Name}";
        }

        public void AddParameter(AttributeParameterDefinition parameter)
        {
            this.Parameters.Add(parameter);
        }
    }
}