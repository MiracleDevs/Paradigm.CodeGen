using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Configuration
{
    [DataContract]
    public class InputConfiguration
    {
        [DataMember]
        public string InputType { get; set; }

        [DataMember]
        public List<InputConfigurationParameter> Parameters { get; set; }

        public InputConfiguration()
        {
            this.Parameters = new List<InputConfigurationParameter>();
        }
    }
}