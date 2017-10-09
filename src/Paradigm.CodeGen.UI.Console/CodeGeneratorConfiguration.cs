using System.Runtime.Serialization;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.CodeGen.Output.Models.Configuration;

namespace Paradigm.CodeGen.UI.Console
{
    [DataContract]
    public class CodeGeneratorConfiguration
    {
        [DataMember]
        public InputConfiguration Input { get; set; }

        [DataMember]
        public OutputConfiguration Output { get; set; }
    }
}