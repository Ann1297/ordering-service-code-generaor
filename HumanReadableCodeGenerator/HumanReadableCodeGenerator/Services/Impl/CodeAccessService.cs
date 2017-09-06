using HumanReadableCodeGenerator.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HumanReadableCodeGenerator.Services
{
    public class CodeAccessService : ICodeAccessService
    {
        private IHostingEnvironment _env;
        private ProjectOptions _options;
        private string _fullPath;

        public CodeAccessService(IHostingEnvironment env, IOptions<ProjectOptions> options)
        {
            _env = env;
            _options = options.Value;
            _fullPath = $"{_env.ContentRootPath}{_options.FilePath}";
        }
        public void Write(List<string> codes)
        {
            if (!IsGeneratedCodeExists())
            {
                File.WriteAllLines(_fullPath, codes);
            }
        }

        public bool IsGeneratedCodeExists()
        {
            return File.Exists(_fullPath);
        }

        public string Get(int id)
        {
            int toSkip = id <= _options.CodesCount ? id - 1 : (id % _options.CodesCount) - 1;
            string code = File.ReadLines(_fullPath).Skip(toSkip).Take(1).First();

            return AddPrefix(code);
        }
         
        private string AddPrefix(string code)
        {
            string prefix = _options.ConstantPrefix;

            if (prefix.Contains("%month%"))
            {
                prefix = prefix.Replace("%month%", DateTime.Now.Month.ToString());
            }
            if (prefix.Contains("%year%"))
            {
                prefix = prefix.Replace("%year%", DateTime.Now.Year.ToString());
            }

            return $"{prefix}{code}";
        }
    }
}
