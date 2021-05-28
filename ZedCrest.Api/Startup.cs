using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Data;
using ZedCrest.Api.Handler;
using ZedCrest.Api.Models;

namespace ZedCrest.Api
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZedCrest.Api", Version = "v1" });
            });

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddDbContext<ZedCrestContext>(options => options.UseInMemoryDatabase(databaseName: "ZedCrest"));
            services.AddHttpContextAccessor();

            var emailSettings = new EmailSettings();
            Configuration.GetSection(nameof(EmailSettings)).Bind(emailSettings);
            services.AddSingleton(emailSettings);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<FileSubmittedHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration.GetValue<string>("RabbitMq:ConnectionString"));
                    cfg.ReceiveEndpoint("event-listener", e =>
                    {
                        e.ConfigureConsumer<FileSubmittedHandler>(context);
                    });
                });
            });
            services.AddMassTransitHostedService();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZedCrest.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging(x =>
            {
                x.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                };
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
