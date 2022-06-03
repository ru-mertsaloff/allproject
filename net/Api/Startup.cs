using Api.Implementions;
using Api.Interfaces;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            var configuration = Configuration.Get<AppSettings>();

            services.AddScoped<IObservable, Observable>();

            services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")), ServiceLifetime.Singleton);
            services.AddDbContext<DbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PgSqlConnection")), ServiceLifetime.Singleton);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<Commands.Api>();
                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.ClearMessageDeserializers();
                    cfg.UseRawJsonSerializer();

                    cfg.Host(configuration.RabbitMQHost, configuration.RabbitMQVirtualHost, c =>
                    {
                        c.Username(configuration.RabbitMQLogin);
                        c.Password(configuration.RabbitMQPassword);
                    });
                    cfg.ConcurrentMessageLimit = 1;
                    cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance);

                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
