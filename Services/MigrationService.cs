using Microsoft.EntityFrameworkCore;
using SkyTrade.Models;

namespace SkyTrade.Services;

/// <summary>
/// Runs db migrations on start with service scope
/// </summary>
public class MigrationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TradeRequestDBContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
