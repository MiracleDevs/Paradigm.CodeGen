using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class EnumValue
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }      
    }
}