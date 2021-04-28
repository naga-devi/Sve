namespace JxNet.Extensions.WebHost
{
    using JxNet.Extensions.ApiBase.Swagger;
    using JxNet.Extensions.DinkPDF;
    //using JxNet.Extensions.OneSignal;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Sve.Service;
    using System.IO;

    public class Startup : ApiBase.StartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
            DisableAutoAudit = false;
        }

        public override void CustomApplicationBuilder(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var dbInitializer = serviceScope.ServiceProvider.GetRequiredService<IDbInitializer>();
            //    dbInitializer.Migrate();
            //    dbInitializer.Seed();
            //}

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            base.CustomApplicationBuilder(app, env);
        }

        public override void CustomMvcOptions(MvcOptions option)
        {
            option.AllowEmptyInputInBodyModelBinding = false;

            base.CustomMvcOptions(option);
        }

        public override SwaggerInfo GetSwaggerConfig()
        {
            return new SwaggerInfo()
            {
                Version = "v1",
                DocumentName = "SVE Web API v1",
                DocumentTitle = "Swagger UI :: SVE",
                Description = "NPSVE API"
            };
        }

        /// <summary>
        /// Registers an Microsoft.AspNetCore.SpaServices.StaticFiles.ISpaStaticFileProvider
        /// service that can provide static files to be served for a Single Page Application(SPA)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override IServiceCollection RegisterSPAService(IServiceCollection services)
        {
            //In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.RegisterServiceInterfaces(Configuration.GetConnectionString("SQL:Sve"));
            //services.AddOneSignalService(Configuration);
            services.AddDinkToPDFService(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DinkToPdf", "libwkhtmltox.dll"));

            return base.RegisterSPAService(services);
        }
    }
}
