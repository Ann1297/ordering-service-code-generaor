using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HumanReadableCodeGenerator.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HumanReadableCodeGenerator.Controllers
{
    [Route("api/code")]
    public class CodeGeneratorController : Controller
    {
        private ICodeAccessService _accessService;

        public CodeGeneratorController(ICodeAccessService accessService)
        {
            _accessService = accessService;
        }

        // GET: api/values
        [HttpGet]
        [Route("{id}")]
        public string Get(int id)
        {
            return _accessService.Get(id);
        }        
    }
}
