using DiscoveringAsp.netCore.Dtos;
using DiscoveringAsp.netCore.Entities;
using DiscoveringAsp.netCore.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace DiscoveringAsp.netCore

{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            env.ConfigureNLog("nlog.config");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {

                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddDbContext<DiscoveringAspnetCoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info() { Title = "My First WebAPI in .NET Core 2.1", Version = "v1" });
            });

            services.AddMvc(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                config.InputFormatters.Add(new XmlSerializerInputFormatter());

            });

            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");

            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug(LogLevel.Error);

            loggerFactory.AddNLog();

            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {

                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";

                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (errorFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global expection logger");

                            logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                        }
                        await context.Response.WriteAsync("There was an error");

                    });
                });
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My First WebAPI");
            });

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerCreateDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            });

            app.UseMvc();




        }
    }
}