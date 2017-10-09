using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Json.Models
{
    [DataContract]
    public class Property: MemberBase
    {
        public override string ToString()
        {
            return $"property {this.TypeName} {this.Name}";
        }
    }
}