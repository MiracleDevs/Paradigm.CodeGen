using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class MethodDefinition
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ObjectDefinitionBase ReturnType { get; set; }

        [DataMember]
        public List<ParameterDefinition> Parameters { get; set; }

        [DataMember]
        public List<AttributeDefinition> Attributes { get; set; }

        public MethodDefinition()
        {
            this.Parameters = new List<ParameterDefinition>();
            this.Attributes = new List<AttributeDefinition>();
        }

        public void AddAttribute(AttributeDefinition definition)
        {
            this.Attributes.Add(definition);
        }

        public void AddParameter(ParameterDefinition definition)
        {
            this.Parameters.Add(definition);
        }

        public override string ToString()
        {
            return $"method {this.ReturnType.Name} {this.Name}()";
        }
    }
}