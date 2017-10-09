using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class SummaryFileConfiguration
    {
        [DataMember]
        public string OutputFileName { get; set; }

        [DataMember]
        public string TemplatePath { get; set; }

        [DataMember]
        public List<OutputConfigurationParameter> Parameters { get; set; }

        public string this[string name]
        {
            get
            {
                var entry = this.Parameters.FirstOrDefault(x => x.Name == name);

                if (entry == null)
                    throw new Exception($"Parameter {name} does not exist in output configuration.");

                return entry.Value;
            }
        }

        public SummaryFileConfiguration()
        {
            this.Parameters = new List<OutputConfigurationParameter>();
        }

        public void AddParameter(OutputConfigurationParameter parameter)
        {
            this.Parameters?.Add(parameter);
        }
    }
}