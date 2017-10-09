using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public abstract class MemberDefinitionBase
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ObjectDefinitionBase Type { get; set; }

        [DataMember]
        public List<AttributeDefinition> Attributes { get; set; }

        protected MemberDefinitionBase()
        {
            this.Attributes = new List<AttributeDefinition>();
        }

        public void AddAttribute(AttributeDefinition definition)
        {
            this.Attributes.Add(definition);
        }
    }
}