using PriceTracker.Persistence;

namespace PriceTracker.API.BackgroundServices;

public sealed class DeleteExpiredConsumedRefreshTokensHostedService : BackgroundService
{
    private readonly ILogger<DeleteExpiredConsumedRefreshTokensHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DeleteExpiredConsumedRefreshTokensHostedService(ILogger<DeleteExpiredConsumedRefreshTokensHostedService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

        await DoWorkAsync(cancellationToken);

        while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
        {
            try
            {
                await DoWorkAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting expired consumed refresh tokens");
            }
        }
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var expiredRefreshTokens = dbContext.ConsumedRefreshTokens.Where(x => x.ExpiresAt < DateTime.UtcNow).ToList();

        if (expiredRefreshTokens.Count == 0)
        {
            _logger.LogInformation("No expired consumed refresh tokens found");
            return;
        }

        _logger.LogInformation("Deleting expired consumed refresh tokens");
        dbContext.ConsumedRefreshTokens.RemoveRange(expiredRefreshTokens);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted {Count} expired consumed refresh tokens", expiredRefreshTokens.Count);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete Expired Consumed Refresh Tokens Hosted Service is running");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete Expired Consumed Refresh Tokens Hosted Service is stopping");
        return base.StopAsync(cancellationToken);
    }
}