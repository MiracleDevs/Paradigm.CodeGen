using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Tests.Mocks
{
    [DataContract]
    public class TestItem
    {
        [DataMember]
        public int Integer { get; set; }

        [DataMember]
        public string String { get; set; }

        [DataMember]
        public bool Boolean { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public List<int> List { get; set; }
    }
}
