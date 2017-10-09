using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class TypeMatcherConfiguration
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string[] Parameters { get; set; }

        [DataMember]
        public bool Negate { get; set; }

        public TypeMatcherConfiguration()
        {
            this.Parameters = new string[0];
        }
    }
}
