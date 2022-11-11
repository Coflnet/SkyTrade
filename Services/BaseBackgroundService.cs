using System.Threading;
using System.Threading.Tasks;
using Coflnet.Sky.Trade.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Logging;
using Coflnet.Sky.Trade.Controllers;
using Coflnet.Sky.Core;

namespace Coflnet.Sky.Trade.Services;

public class BaseBackgroundService : BackgroundService
{
    private IServiceScopeFactory scopeFactory;
    private IConfiguration config;
    private ILogger<BaseBackgroundService> logger;
    private Prometheus.Counter consumeCount = Prometheus.Metrics.CreateCounter("sky_base_conume", "How many messages were consumed");

    public BaseBackgroundService(
        IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<BaseBackgroundService> logger)
    {
        this.scopeFactory = scopeFactory;
        this.config = config;
        this.logger = logger;
    }
    /// <summary>
    /// Called by asp.net on startup
    /// </summary>
    /// <param name="stoppingToken">is canceled when the applications stops</param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var flipCons = Coflnet.Kafka.KafkaConsumer.ConsumeBatch<LowPricedAuction>(config["KAFKA_HOST"], config["TOPICS:LOW_PRICED"], async batch =>
        {
            var service = GetService();
            foreach (var lp in batch)
            {
                // do something
            }
            consumeCount.Inc(batch.Count());
        }, stoppingToken, "SkyTrade");

        await Task.WhenAll(flipCons);
    }

    private BaseService GetService()
    {
        return scopeFactory.CreateScope().ServiceProvider.GetRequiredService<BaseService>();
    }
}