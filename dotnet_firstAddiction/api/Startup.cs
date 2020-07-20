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

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.For<IDbManager>().Use(new DbManager("localhost", "root", "root", "FirstAddiction"));
            
            var container = new Container(services);

            // services.For(typeof(FirstAddictionContext)).Use(new FirstAddictionContext(container.GetInstance<IDbManager>()));
            services.For<FirstAddictionContext>().Use(new FirstAddictionContext(container.GetInstance<IDbManager>()));

            services.ForConcreteType<FirstAddictionContext>();

            services.Scan(s => {
               s.TheCallingAssembly();
               s.WithDefaultConventions();
           });   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
