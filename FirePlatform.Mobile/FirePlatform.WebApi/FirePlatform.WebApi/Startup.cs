using AutoMapper;
using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using FirePlatform.Services;
using FirePlatform.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            //services.AddSwaggerGen();

            #region IoC
            
            //repositories
            services.AddScoped<Repository, Repository>();

            services.AddScoped<FormRepository, FormRepository>();
            services.AddScoped<BaseRepository<Form, FormRepository>, BaseRepository<Form, FormRepository>>();
            services.AddScoped<UserFormRepository, UserFormRepository>();
            services.AddScoped<BaseRepository<UserForm, UserFormRepository>, BaseRepository<UserForm, UserFormRepository>>();
            services.AddScoped<UserRepository, UserRepository>();
            services.AddScoped<BaseRepository<User, UserRepository>, BaseRepository<User, UserRepository>>();

            //services
            services.AddScoped<Service, Service>();

            services.AddScoped<UserService, UserService>();
            services.AddScoped<BaseService<UserService, UserRepository, User>, BaseService<UserService, UserRepository, User>>();
            services.AddScoped<FormService, FormService>();
            services.AddScoped<BaseService<FormService, FormRepository, Form>, BaseService<FormService, FormRepository, Form>>();


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

            //app.UseHttpsRedirection();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            // app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            //app.UseSwaggerUi();

        }
    }
}
