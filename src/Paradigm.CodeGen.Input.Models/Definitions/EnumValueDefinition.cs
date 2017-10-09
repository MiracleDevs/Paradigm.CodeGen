using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class EnumValueDefinition
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }      
    }
}