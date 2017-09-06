using MatOrderingService.Services.CodeGenerator;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MatOrderingService.Services
{
    public class OrderCodeGenerator : IOrderCodeGenerator
    {
        private CodeGeneratorOptions _options;

        public OrderCodeGenerator(IOptions<CodeGeneratorOptions> options)
        {
            _options = options.Value;
        }
        public async Task<string> Get(int id)
        {
            using (var client = new HttpClient())
            {
                string path = $"{_options.Host}{_options.Path}{id}";
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
        }
    }
}
