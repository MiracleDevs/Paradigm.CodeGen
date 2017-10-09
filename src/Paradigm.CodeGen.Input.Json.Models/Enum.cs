using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Enum : ObjectBase
    {      
        [DataMember]
        public List<EnumValue> Values { get; set; }

        public Enum()
        {
            this.Values = new List<EnumValue>();
        }

        public void AddValue(EnumValue definition)
        {
            this.Values.Add(definition);
        }

        public override string ToString()
        {
            return $"enum {this.Name}";
        }
    }
}