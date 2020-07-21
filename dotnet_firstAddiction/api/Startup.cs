using System.Collections.Generic;
using System.Reflection;
using System.Text;
using api.common.Db;
using api.common.Security;
using api.Models;
using AutoMapper;
using GraphiQl;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("UserPolicy",
            //                     policy => policy.RequireClaim("sub"));
            // });
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            //Hard coded for now but should probably read from a config file.
            services.For<IDbManager>()
                .Use(new DbManager(
                    Configuration["Db:server"],
                    Configuration["Db:username"],
                    Configuration["Db:password"],
                    Configuration["Db:name"]));

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
            // app.UseAuthorization();
            app.UseAuthentication();
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
