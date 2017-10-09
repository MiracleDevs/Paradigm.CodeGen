using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public abstract class MemberBase
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public List<Attribute> Attributes { get; set; }

        protected MemberBase()
        {
            this.Attributes = new List<Attribute>();
        }

        public void AddAttribute(Attribute definition)
        {
            this.Attributes.Add(definition);
        }
    }
}