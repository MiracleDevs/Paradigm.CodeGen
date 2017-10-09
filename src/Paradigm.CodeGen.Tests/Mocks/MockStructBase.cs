namespace Paradigm.CodeGen.Tests.Mocks
{
    [Mock]
    public struct MockStructBase
    {
        public decimal DecimalValue { get; set; }

        public string StringValue { get; set; }

        public TestClass CustomValue { get; set; }

        private int IntValue { get; set; }

        public void TestMethodVoid()
        {

        }

        public int TestMethodInt()
        {
            return 42;
        }

        public TestClass TestMethodCustom()
        {
            return new TestClass();
        }
    }

}
