using System;

namespace Paradigm.CodeGen.Tests.Mocks
{
    public class MockBaseAttribute : Attribute
    {
        public string PublicAttr => "TestValue";

        protected int ProtectedAttr { get; set; }

        private string PrivateAttr { get; set; }

        public int NumericAttrInt { get; set; }

        public decimal NumericAttrDecimal { get; set; }

        public float NumericAttrFloat { get; set; }

        public double NumericAttrDouble { get; set; }
    }
}