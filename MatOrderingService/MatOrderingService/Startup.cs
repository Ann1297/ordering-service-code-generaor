using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MatOrderingService.Middlewares;
using Microsoft.Extensions.Configuration;
using MatOrderingService.Services.Storage;
using MatOrderingService.Services.Storage.Impl;
using AutoMapper;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authorization;
using MatOrderingService.Services.Auth;
using MatOrderingService.Swagger;
using MatOrderingService.Filters;
using MatOrderingService.Services;
using MatOrderingService.Services.CodeGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MatOrderingService
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<MatOsAuthOptions>(Configuration.GetSection("AuthOptions"));

            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Configuration["AuthOptions:AuthenticationScheme"])
                .RequireAuthenticatedUser().Build();
            });

            services.Configure<MyOptions>(Configuration.GetSection("MySection"));
            services.AddSingleton<IOrderingService, OrderingService>();
            services.AddSingleton<IProductsService, ProductsService>();

            services.Configure<CodeGeneratorOptions>(Configuration.GetSection("CodeGenerator"));
            services.AddSingleton<IOrderCodeGenerator, OrderCodeGenerator>();

            var connectionString = Configuration["Data:ConnectionString"];
            services.AddDbContext<OrderDbContext>(options => options
                .UseSqlServer(connectionString)
                .ConfigureWarnings(warning => warning.Log(RelationalEventId.QueryClientEvaluationWarning)));

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(EntityNotFoundExceptionFilter));
                options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Materialise Academy Orders Api", Version = "v1"});
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MatOrderingService.xml");
                c.IncludeXmlComments(filePath);
                c.OperationFilter<SwaggerAuthorizationHeaderParameter>(Configuration["AuthOptions:AuthenticationScheme"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseLoggingMiddleware();

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
                        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<MatOsAuthMiddleware>();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Materialise Academy Orders Api");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(Configuration["Environment:WelcomeMessage"]);
            });
        }
    }
}
