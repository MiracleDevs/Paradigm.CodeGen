using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class AttributeParameterDefinition
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public bool IsNumeric { get; set; }

        public override string ToString()
        {
            return $"parameter {this.Name}: {this.Value}";
        }
    }
}