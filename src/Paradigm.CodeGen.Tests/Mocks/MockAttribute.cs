namespace Paradigm.CodeGen.Tests.Mocks
{
    public class MockAttribute : MockBaseAttribute
    {
        public string PublicAttr2 => "TestValue";

        protected int ProtectedAttr2 { get; set; }

        private string PrivateAttr2 { get; set; }
    }
}
