using System.Collections.Generic;

namespace HumanReadableCodeGenerator.Services
{
    public interface ICodeAccessService
    {
        string Get(int id);
        void Write(List<string> codes);
        bool IsGeneratedCodeExists();
    }
}