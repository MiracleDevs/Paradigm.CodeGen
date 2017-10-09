using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class OutputConfiguration
    {
        [DataMember]
        public string OutputType { get; set; }

        [DataMember]
        public SummaryFileConfiguration Summary { get; set; }

        [DataMember]
        public List<OutputConfigurationParameter> Parameters { get; }

        [DataMember]
        public List<OutputFileConfiguration> OutputFiles { get; }

        public OutputConfiguration()
        {
            this.Parameters = new List<OutputConfigurationParameter>();
            this.OutputFiles = new List<OutputFileConfiguration>();
        }

        public void AddParameter(OutputConfigurationParameter parameter)
        {
            this.Parameters?.Add(parameter);
        }

        public void AddOutputFile(OutputFileConfiguration file)
        {
            this.OutputFiles?.Add(file);
        }
    }
}