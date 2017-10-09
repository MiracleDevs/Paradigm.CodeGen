using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Tests.Mocks
{
    [Mock]
    public abstract class TestAbstractClass 
    {
        [DataMember]
        public List<TestItem> Items { get; set; }

        internal List<TestItem> InternalItems { get; set; }

        protected List<string> ProtectedItems { get; set; }

        private List<string> PrivateItems { get; set; }

        public static List<TestItem> PublicStaticItems { get; set; }

        protected static List<string> ProtectedStaticItems { get; set; }

        private static List<string> PrivateStaticItems { get; set; }

        public void PublicMethod() { }

        internal void InternalMethod() { }

        protected void ProtectedMethod() { }

        private void PrivateMethod() { }

        public static void PublicStaticMethod() { }

        protected static void ProtectedStaticMethod() { }

        private static void PrivateStaticMethod() { }
    }
}

