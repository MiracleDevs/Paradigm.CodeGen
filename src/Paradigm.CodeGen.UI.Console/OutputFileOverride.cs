namespace Paradigm.CodeGen.UI.Console
{
    public class OutputFileOverride
    {
        public string OutputFileConfigurationName { get; }

        public string TypeName { get; }

        public OutputFileOverride(string configuration)
        {
            var values = configuration.Split(':');

            if (values.Length != 2) 
                return;

            this.OutputFileConfigurationName = values[0];
            this.TypeName = values[1];
        }
    }
}