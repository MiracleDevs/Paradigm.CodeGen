using System.Runtime.Serialization;

namespace Paradigm.CodeGen.Input.Models.Translations
{
    [DataContract]
    public class NativeTypeTranslator
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Translation { get; set; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Translation}";
        }
    }
}