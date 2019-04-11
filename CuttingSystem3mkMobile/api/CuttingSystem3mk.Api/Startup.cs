using AutoMapper;
using CuttingSystem3mk.Repositories.Repositories;
using CuttingSystem3mk.Services.Services;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using CuttingSystem3mkMobile.Services;
using CuttingSystem3mkMobile.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CuttingSystem3mkMobile.Api
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

            services.AddScoped<DeviceModelRepository, DeviceModelRepository>();
            services.AddScoped<BaseRepository<DeviceModel, DeviceModelRepository>, BaseRepository<DeviceModel, DeviceModelRepository>>();

            services.AddScoped<CutCodesRepository, CutCodesRepository>();
            services.AddScoped<BaseRepository<CutCode, CutCodesRepository>, BaseRepository<CutCode, CutCodesRepository>>();

            //services
            services.AddScoped<Service, Service>();

            services.AddScoped<UserService, UserService>();
            services.AddScoped<BaseService<UserService, UserRepository, User>, BaseService<UserService, UserRepository, User>>();

            services.AddScoped<CutModelService, CutModelService>();
            services.AddScoped<BaseService<CutModelService, CutModelRepository, CutModel>, BaseService<CutModelService, CutModelRepository, CutModel>>();

            services.AddScoped<DeviceModelService, DeviceModelService>();
            services.AddScoped<BaseService<DeviceModelService, DeviceModelRepository, DeviceModel>, BaseService<DeviceModelService, DeviceModelRepository, DeviceModel>>();

            services.AddScoped<CutCodeService, CutCodeService>();
            services.AddScoped<BaseService<CutCodeService, CutCodesRepository, CutCode>, BaseService<CutCodeService, CutCodesRepository, CutCode>>();

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
