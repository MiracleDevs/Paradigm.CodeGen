using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Class : Struct
    {
        public override string ToString()
        {
            return $"class {this.Name}";
        }
    }
}