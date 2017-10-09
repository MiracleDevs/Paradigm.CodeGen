using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Configuration
{
    [DataContract]
    public class InputConfigurationParameter 
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]    
        public string Value { get; set; }
    }
}