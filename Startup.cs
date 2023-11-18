using System.Reflection;
using AutoMapper;
using Coflnet.Sky.Core;
using Coflnet.Sky.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Prometheus;
using SkyTrade.Models;
using SkyTrade.Models.Mappers;
using SkyTrade.Services;

namespace Coflnet.Sky.Trade;
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkyTrade", Version = "v1" });
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        // Replace 'YourDbContext' with the name of your own DbContext derived class.
        services.AddDbContext<TradeRequestDBContext>(options => options.UseNpgsql(Configuration.GetConnectionString("CockRoachDB")));
        services.AddAutoMapper(typeof(TradeRequestProfile));

        services.AddTransient<FilterEngine>();
        services.AddTransient<IDBService, DBService>();
        services.AddJaeger(Configuration);
        services.AddResponseCaching();
        services.AddResponseCompression();
        services.AddHostedService<MigrationService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseExceptionHandler(errorApp =>
            {
                ErrorHandler.Add(errorApp, "-trade");
            });
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyTrade v1");
            c.RoutePrefix = "api";
        });

        app.UseResponseCaching();
        app.UseResponseCompression();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
            endpoints.MapControllers();
        });

        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}