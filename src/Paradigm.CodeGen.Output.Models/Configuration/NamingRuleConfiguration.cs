using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class NamingRuleConfiguration
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string[] Parameters { get; set; }

        public NamingRuleConfiguration()
        {
            this.Parameters = new string[0];
        }
    }
}