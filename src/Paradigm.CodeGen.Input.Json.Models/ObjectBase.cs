using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public abstract class ObjectBase
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public bool IsInterface { get; set; }

        [DataMember]
        public bool IsArray { get; set; }

        [DataMember]
        public bool IsAbstract { get; set; }

        [DataMember]
        public string BaseTypeName { get; set; }

        [DataMember]
        public string InnerObjectName { get; set; }

        [DataMember]
        public List<string> ImplementedInterfaces { get; set; }

        [DataMember]
        public List<Attribute> Attributes { get; set; }

        protected ObjectBase()
        {
            this.Attributes = new List<Attribute>();
            this.ImplementedInterfaces = new List<string>();
        }

        public void AddImplementedInterface(string definition)
        {
            this.ImplementedInterfaces?.Add(definition);
        }

        public void AddAttribute(Attribute definition)
        {
            this.Attributes?.Add(definition);
        }
    }
}