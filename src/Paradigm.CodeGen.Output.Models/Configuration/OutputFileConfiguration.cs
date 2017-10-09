using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Output.Models.Configuration
{
    [DataContract]
    public class OutputFileConfiguration
    {
        [IgnoreDataMember]
        public string ExecutingFullPath => Path.GetDirectoryName(typeof(OutputFileConfiguration).GetTypeInfo().Assembly.Location);

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string TemplatePath { get; set; }

        [DataMember]
        public string OutputPath { get; set; }

        [DataMember]
        public SummaryFileConfiguration Summary { get; set; }

        [DataMember]
        public List<NamingRuleConfiguration> NamingRules { get; set; }

        [DataMember]
        public List<TypeMatcherConfiguration> TypeMatchers { get; set; }

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

        public OutputFileConfiguration()
        {
            this.NamingRules = new List<NamingRuleConfiguration>();
            this.TypeMatchers = new List<TypeMatcherConfiguration>();
            this.Parameters = new List<OutputConfigurationParameter>();
        }

        public void AddParameter(OutputConfigurationParameter parameter)
        {
            this.Parameters?.Add(parameter);
        }

        public void AddNamingRules(NamingRuleConfiguration namingRule)
        {
            this.NamingRules?.Add(namingRule);
        }

        public void AddTypeMatchers(TypeMatcherConfiguration typeMatcher)
        {
            this.TypeMatchers?.Add(typeMatcher);
        }
    }
}