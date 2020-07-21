using System.Collections.Generic;
using System.Reflection;
using api.common.Db;
using api.common.Security;
using api.Models;
using AutoMapper;
using GraphiQl;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddDbContext<FirstAddictionContext>();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            //Hard coded for now but should probably read from a config file.
            services.For<IDbManager>()
                .Use(new DbManager("localhost", "root", "root", "FirstAddiction"));

            services.For<IPasswordVerifier>().Use<PasswordVerifier>();

            services.For<DbContext>().Use<FirstAddictionContext>();



            services.ForConcreteType<FirstAddictionContext>();

            services.Scan(s => {
               s.TheCallingAssembly();
               s.WithDefaultConventions();
               s.AddAllTypesOf<Profile>();
           });   

            var container = new Container(services);

            var profiles = container.GetAllInstances<Profile>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(profiles);
            });

            services.For<IMapper>()
                .Use(new Mapper(config));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGraphiQl("/graphiql");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
