using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public abstract class ObjectDefinitionBase
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public bool IsArray { get; set; }

        [DataMember]
        public bool IsInterface { get; set; }

        [DataMember]
        public bool IsAbstract { get; set; }

        [DataMember]
        public ObjectDefinitionBase BaseType { get; set; }

        [DataMember]
        public ObjectDefinitionBase InnerObject { get; set; }

        [DataMember]
        public List<ObjectDefinitionBase> ImplementedInterfaces { get; set; }

        [DataMember]
        public List<AttributeDefinition> Attributes { get; set; }

        protected ObjectDefinitionBase()
        {
            this.Attributes = new List<AttributeDefinition>();
            this.ImplementedInterfaces = new List<ObjectDefinitionBase>();
        }

        public void AddImplementedInterface(ObjectDefinitionBase definition)
        {
            this.ImplementedInterfaces?.Add(definition);
        }

        public void AddAttribute(AttributeDefinition definition)
        {
            this.Attributes?.Add(definition);
        }
    }
}