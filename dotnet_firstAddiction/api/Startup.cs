using api.common.Db;
using api.Models;
using GraphiQl;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            
            var container = new Container(services);


            services.ForConcreteType<FirstAddictionContext>();

            services.Scan(s => {
               s.TheCallingAssembly();
               s.WithDefaultConventions();
           });   
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
