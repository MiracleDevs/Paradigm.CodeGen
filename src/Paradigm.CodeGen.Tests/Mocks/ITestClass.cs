using System.Collections.Generic;

namespace Paradigm.CodeGen.Tests.Mocks
{
    public interface ITestClass
    {
        List<TestItem> Items { get; set; }

        void PublicMethod();
    }
}