using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.IoC;
using Passenger.Infrastructure.IoC.Modules;
using Passenger.Infrastructure.Mappers;
using Passenger.Infrastructure.Repositories;
using Passenger.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Passenger.Infrastructure.Settings;
using Newtonsoft.Json;
using Passenger.Api.Framework;
using NLog.Extensions.Logging;
using NLog.Web;
using Passenger.Infrastructure.Mongo;
using Passenger.Infrastructure.EF;

namespace Passenger.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration{get;}
        public IContainer ApplicationContainer{get; private set;}
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(x=> x.AddPolicy("admin",p=>p.RequireRole("admin")));
            services.AddMemoryCache();
            // Add framework services.           
            services.AddMvc().AddJsonOptions(x=> x.SerializerSettings.Formatting = Formatting.Indented);
            // services.AddEntityFrameworkSqlServer()
            //     .AddEntityFrameworkInMemoryDatabase()
            //     .AddDbContext<PassengerContext>();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory,IApplicationLifetime appLifeTime)
        {
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug();

            loggerFactory.AddNLog();
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");

            var jwtSettings = app.ApplicationServices.GetService<JwtSettings>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions{
                AutomaticAuthenticate=true,
                TokenValidationParameters=new TokenValidationParameters{
                    ValidIssuer=jwtSettings.Issuer,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                }
            });

            MongoConfigurator.Initialize();

            var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
            if(generalSettings.SeedData)
            {
                var dataInitializer = app.ApplicationServices.GetService<IDataInitializer>();
                dataInitializer.SeedAsync();
            }
            app.UseExceptionsHandler();
            app.UseMvc();
            appLifeTime.ApplicationStopped.Register(()=> ApplicationContainer.Dispose());
        }
    }
}
