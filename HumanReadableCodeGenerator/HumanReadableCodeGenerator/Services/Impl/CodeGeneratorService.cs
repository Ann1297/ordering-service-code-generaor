using HumanReadableCodeGenerator.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanReadableCodeGenerator.Services
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        private string[] _vowels;
        private string[] _consonants;
        private Random _random;
        private ProjectOptions _options;
        private IHostingEnvironment _env;

        public CodeGeneratorService(IOptions<ProjectOptions> options, IHostingEnvironment env)
        {
            //_vowels = "A,AI,AU,E,EA,EE,I,IA,IO,O,OA,OI,OO,OU,U".Split(',');
            //_consonants = "B,C,CH,CL,D,F,FF,G,GH,GL,J,K,L,LL,M,MN,N,P,PH,PS,R,RH,S,SC,SH,SK,ST,T,TH,V,W,X,Y,Z"
            //    .Split(',');

            _vowels = "A,E,I,O,U".Split(',');
            _consonants = "B,C,D,F,G,H,J,K,L,M,N,P,Q,R,S,T,V,W,X,Y,Z".Split(',');

            _random = new Random();
            _options = options.Value;
            _env = env;
        }
        
        public List<string> GenerateMany(int count)
        {
            List<string> codes = new List<string>();

            while (codes.Count < _options.CodesCount)
            {
                var code = GenerateOne(_options.CodeLength);
                if (codes.Count == 0 
                    || codes.FirstOrDefault(c => c == code) == null)
                {
                    codes.Add(code);
                }
            }
            
            return codes;
        }


        public string GenerateOne(int length)
        {
            StringBuilder sb = new StringBuilder(0, length);

            bool isVowel = _random.Next(2) == 0;

            while (length > 0)
            {
                length -= AddElement(sb, isVowel ? _vowels : _consonants);
                isVowel = !isVowel;
            }

            return sb.ToString();
        }

        private int AddElement(StringBuilder sb, string[] letters)
        {
            string element = letters[_random.Next(letters.Length)];
                        
            if (element.Length + sb.Length > sb.MaxCapacity)
            {
                element = element.Substring(0, sb.MaxCapacity - sb.Length);
            }

            sb.Append(element);
            return element.Length;
        }
    }
}
