using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Tests.Mocks
{
    [Mock]
    public class TestClassExtends : TestClass
    {
        [DataMember]
        public override List<TestItem> VirtualItems{ get; set; }

        public List<TestItem> Items2 { get; set; }

        [DataMember]
        private List<TestItem> PrivateItemsExtended { get; set; }

        internal List<TestItem> InternalItemsExtended { get; set; }

        protected List<string> ProtectedItemsExtended { get; set; }

        protected static List<string> ProtectedStaticItemsExtended { get; set; }

        private static List<string> PrivateStaticItemsExtended { get; set; }


        public override void PublicVirtualMethod()
        {
            base.PublicVirtualMethod();
        }

        public void PublicMethod2()
        {
        }

        internal void InternalMethodExtended() { }

        protected void ProtectedMethodExtended() { }

        private void PrivateMethodExtended() { }

        protected static void ProtectedStaticMethodExtended() { }

        private static void PrivateStaticMethodExtended() { }

    }
}