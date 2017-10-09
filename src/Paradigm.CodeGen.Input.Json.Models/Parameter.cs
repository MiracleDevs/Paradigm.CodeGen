using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Parameter: MemberBase
    {
        public override string ToString()
        {
            return $"parameter {this.TypeName} {this.Name}";
        }
    }
}