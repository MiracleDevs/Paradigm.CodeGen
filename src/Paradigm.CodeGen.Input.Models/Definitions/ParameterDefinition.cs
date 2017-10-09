using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class ParameterDefinition: MemberDefinitionBase
    {
        public override string ToString()
        {
            return $"parameter {this.Type.Name} {this.Name}";
        }
    }
}