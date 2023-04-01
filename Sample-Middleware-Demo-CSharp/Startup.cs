using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HexaEightEngine;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.IO;
using System.Text;
using System;

namespace HexaEight_Middleware_SampleDemo
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
            services.Add(new ServiceDescriptor(typeof(HexaEightResource), new HexaEightResource("87ADEEA59F56FE1427ABC682763DF8474BADFDBC", "http://216.250.114.110:5000")));
            //services.Add(new ServiceDescriptor(typeof(HexaEightResource), new HexaEightResource("C6A9F547971F0DCA373D092595D6B71E208A6768", "http://216.250.114.110:5000")));
            services.AddSingleton<IAuthenticationService, AuthenticationMiddleware>();
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Enable CORS for all origns including credentials. We will validate the user agents in the authentication section
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                .WithExposedHeaders("Provider")
                .WithExposedHeaders("X-Content-Length")
                .WithExposedHeaders("X-Content-Type")
                .WithExposedHeaders("X-Suggested-Filename")
                .WithExposedHeaders("Content-Disposition"));

            //Add HexaEight Middleware first in the pipeline so that valid request can be authenticated.

            app.UseMiddleware<HexaEightEngine.Middleware>();
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                //if (context.Request.Method.Equals("options", StringComparison.InvariantCultureIgnoreCase) && context.Request.Headers.ContainsKey("Access-Control-Request-Private-Network"))
                //{
                //    context.Response.Headers.Add("Access-Control-Allow-Private-Network", "true");
                //}

                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("UnAuthorized - Access Denied"));
                }

            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
