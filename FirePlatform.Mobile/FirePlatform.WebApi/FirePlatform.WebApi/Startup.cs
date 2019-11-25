using AutoMapper;
using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using FirePlatform.Services;
using FirePlatform.Services.Services;
using FirePlatform.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FirePlatform.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });


            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p
                                                                    .AllowAnyOrigin()
                                                                    .AllowAnyMethod()
                                                                     .AllowAnyHeader()
                                                                     .AllowCredentials()
                                                                     .WithOrigins("http://localhost:8081", "http://localhost:8080",
                                                                      "http://localhost:8082", "http://fireplatform.herokuapp.com")
                                                                     ));


            services.AddSession();

            //.AllowCredentials()));

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            //services.AddSwaggerGen();

            #region IoC

            //repositories
            services.AddScoped<Repository, Repository>();

            services.AddScoped<FormRepository, FormRepository>();
            services.AddScoped<BaseRepository<TemplateModel, FormRepository>, BaseRepository<TemplateModel, FormRepository>>();

            services.AddScoped<UserFormRepository, UserFormRepository>();
            services.AddScoped<BaseRepository<UserForm, UserFormRepository>, BaseRepository<UserForm, UserFormRepository>>();

            services.AddScoped<UserRepository, UserRepository>();
            services.AddScoped<BaseRepository<User, UserRepository>, BaseRepository<User, UserRepository>>();

            services.AddScoped<UserTemplatesRepository, UserTemplatesRepository>();
            services.AddScoped<BaseRepository<UserTemplates, UserTemplatesRepository>, BaseRepository<UserTemplates, UserTemplatesRepository>>();

            //services
            services.AddScoped<Service, Service>();

            services.AddScoped<UserService, UserService>();
            services.AddScoped<BaseService<UserService, UserRepository, User>, BaseService<UserService, UserRepository, User>>();

            services.AddScoped<FormService, FormService>();
            services.AddScoped<BaseService<FormService, FormRepository, TemplateModel>, BaseService<FormService, FormRepository, TemplateModel>>();

            services.AddScoped<UserTemplatesService, UserTemplatesService>();
            services.AddScoped<BaseService<UserTemplatesService, UserTemplatesRepository, UserTemplates>, BaseService<UserTemplatesService, UserTemplatesRepository, UserTemplates>>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseHsts();

            var defaultDateCulture = "en-US";
            var ci = new CultureInfo(defaultDateCulture);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
    {
        ci,
    },
                SupportedUICultures = new List<CultureInfo>
    {
        ci,
    }
            });

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();

            app.UseCors("AllowAll");
            // Enable middleware to serve generated Swagger as a JSON endpoint
            // app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            //app.UseSwaggerUi();

            //set culture


        }
    }
}
