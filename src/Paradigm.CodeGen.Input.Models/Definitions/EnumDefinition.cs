using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class EnumDefinition : ObjectDefinitionBase
    {      
        [DataMember]
        public List<EnumValueDefinition> Values { get; set; }

        public EnumValueDefinition this[string valueName]
        {
            get
            {
                if (valueName == null)
                    throw new System.ArgumentNullException(nameof(valueName));

                return this.Values.FirstOrDefault(x => x.Name == valueName);
            }
        }

        public EnumDefinition()
        {
            this.Values = new List<EnumValueDefinition>();
        }

        public void AddValue(EnumValueDefinition definition)
        {
            this.Values.Add(definition);
        }

        public override string ToString()
        {
            return $"enum {this.Name}";
        }
    }
}