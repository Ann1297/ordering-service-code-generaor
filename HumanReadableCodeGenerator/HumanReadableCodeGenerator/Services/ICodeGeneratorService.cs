using System.Collections.Generic;

namespace HumanReadableCodeGenerator.Services
{
    public interface ICodeGeneratorService
    {
        List<string> GenerateMany(int count);
        string GenerateOne(int length);

        
    }
}