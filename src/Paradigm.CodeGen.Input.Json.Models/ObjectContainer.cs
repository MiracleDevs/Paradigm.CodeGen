using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class ObjectContainer
    {
        [DataMember]
        public Class Class { get; set; }

        [DataMember]
        public Struct Struct { get; set; }

        [DataMember]
        public Enum Enum { get; set; }

        [IgnoreDataMember]
        public ObjectBase Object => this.Class ?? (ObjectBase)this.Struct ?? this.Enum;
    }
}