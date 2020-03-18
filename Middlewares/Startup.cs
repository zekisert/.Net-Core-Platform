using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CustomMiddlewares
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if(context.Request.Path=="/products")
                {
                    var value = context.Request.Query["category"].ToString();
                    
                    if(int.TryParse(value,out int intValue))
                    {
                        await context.Response.WriteAsync($"category sayısal bir ifade : {intValue}");
                    }
                    else
                    {
                        context.Items["value"] = value;
                        await next();
                    }
                }
                else
                {
                    await next();
                }

            });

            app.Use(async (context, next) =>
            {
                if (context.Items["value"] != null)
                {
                    var value = context.Items["value"].ToString();
                    context.Items["value"] = value.ToLower();
                }
                await next();
            }); 


            app.Run(async (context) =>
            {
                if (context.Items["value"]!=null)
                {
                    var value = context.Items["value"].ToString();
                    await context.Response.WriteAsync($"category : {value}");
                }
                else
                {
                    await context.Response.WriteAsync("no query string");
                }
                
            });
        }
    }
}
