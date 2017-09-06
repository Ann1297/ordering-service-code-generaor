using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HumanReadableCodeGenerator.Services;
using Microsoft.Extensions.Configuration;
using HumanReadableCodeGenerator.Options;
using Microsoft.Extensions.Options;

namespace HumanReadableCodeGenerator
{
    public class Startup
    {
        private ICodeGeneratorService _codeGenerator;
        private ICodeAccessService _accessService;
        private ProjectOptions _options;

        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<ProjectOptions>(Configuration);
            services.AddSingleton<ICodeGeneratorService, CodeGeneratorService>();
            services.AddSingleton<ICodeAccessService, CodeAccessService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ICodeGeneratorService codeGenerator, ICodeAccessService accessService, IOptions<ProjectOptions> options)
        {
            _codeGenerator = codeGenerator;
            _accessService = accessService;
            _options = options.Value;

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                if (!_accessService.IsGeneratedCodeExists())
                {
                    var codes = _codeGenerator.GenerateMany(_options.CodesCount);
                    _accessService.Write(codes);
                }
                await next.Invoke();
            });

            //app.UseCodeGeneratorMiddleware();

            //app.UseMvc();

            app.Run(async (context) =>
            {
                string path = context.Request.Path;
                if (path.Contains("/api/code/"))
                {
                    int id;
                    if (int.TryParse(path.Substring(path.LastIndexOf('/') + 1), out id))
                    {
                        await context.Response.WriteAsync(_accessService.Get(id));
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Error");
                }
            });
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hey hey/!");
            //});
        }
    }
}
