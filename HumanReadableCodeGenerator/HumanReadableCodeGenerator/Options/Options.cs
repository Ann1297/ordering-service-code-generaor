using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanReadableCodeGenerator.Options
{
    public class ProjectOptions
    {
        public string ConstantPrefix { get; set; }
        public int CodeLength { get; set; }
        public string FilePath { get; set; }
        public int CodesCount { get; set; }
    }
}
