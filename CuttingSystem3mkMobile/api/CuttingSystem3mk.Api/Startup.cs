using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.Repositories;
using CuttingSystem3mk.Repositories.Repositories;
using CuttingSystem3mk.Services;
using CuttingSystem3mk.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CuttingSystem3mk.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();

            //in the future is needed to allow access to api methods by external node.js hosts, for example Vue.js etc
            //services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            //                                                        .AllowAnyMethod()
            //                                                         .AllowAnyHeader()));

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            //services.AddSwaggerGen();


            #region IoC

            //repositories
            services.AddScoped<Repository, Repository>();

            services.AddScoped<UserRepository, UserRepository>();
            services.AddScoped<BaseRepository<User, UserRepository>, BaseRepository<User, UserRepository>>();

            services.AddScoped<CutModelRepository, CutModelRepository>();
            services.AddScoped<BaseRepository<CutModel, CutModelRepository>, BaseRepository<CutModel, CutModelRepository>>();

            //services
            services.AddScoped<Service, Service>();

            services.AddScoped<UserService, UserService>();
            services.AddScoped<BaseService<UserService, UserRepository, User>, BaseService<UserService, UserRepository, User>>();

            services.AddScoped<CutModelService, CutModelService>();
            services.AddScoped<BaseService<CutModelService, CutModelRepository, CutModel>, BaseService<CutModelService, CutModelRepository, CutModel>>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
