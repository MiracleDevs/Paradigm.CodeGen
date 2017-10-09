using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class ClassDefinition : StructDefinition
    {
        public override string ToString()
        {
            return $"class {this.Name}";
        }
    }
}