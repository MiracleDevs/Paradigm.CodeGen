using System.Collections.Generic;
using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Input
{
    public interface IInputService
    {
        void Process(string fileName, InputConfiguration configuration);

        IEnumerable<ObjectDefinitionBase> GetObjectDefinitions();
    }
}