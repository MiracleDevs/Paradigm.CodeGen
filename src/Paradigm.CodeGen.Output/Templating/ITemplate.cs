namespace Paradigm.CodeGen.Output.Templating
{
    public interface ITemplate
    {
        string Body { get; }

        string FileName { get; }

        void Open(string fileName);

        (int lineNumber, string line) GetLine(int lineNumber);
    }
}