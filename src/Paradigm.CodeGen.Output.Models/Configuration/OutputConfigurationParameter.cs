using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class OutputConfigurationParameter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]    
        public string Value { get; set; }
    }
}