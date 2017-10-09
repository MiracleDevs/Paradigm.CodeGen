using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Definitions
{
    [DataContract]
    public class PropertyDefinition: MemberDefinitionBase
    {
        public override string ToString()
        {
            return $"property {this.Type.Name} {this.Name}";
        }
    }
}