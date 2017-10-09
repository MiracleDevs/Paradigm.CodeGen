namespace Paradigm.CodeGen.UI.Console
{
    public class OutputFileOverride
    {
        public string OutputFileConfigurationName { get; }

        public string TypeName { get; }

        public OutputFileOverride(string configuration)
        {
            var values = configuration.Split(':');

            if (values != null && values.Length == 2)
            {
                this.OutputFileConfigurationName = values[0];
                this.TypeName = values[1];
            }
        }

        public OutputFileOverride(string outputFileConfigurationName, string typeName)
        {
            this.OutputFileConfigurationName = outputFileConfigurationName;
            this.TypeName = typeName;
        }
    }
}