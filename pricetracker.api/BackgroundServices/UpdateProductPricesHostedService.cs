using PriceTracker.Entities;
using PriceTracker.Extractor;
using PriceTracker.Persistence;

namespace PriceTracker.API.BackgroundServices;

public sealed class UpdateProductPricesHostedService : BackgroundService
{
    private readonly ILogger<UpdateProductPricesHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public UpdateProductPricesHostedService(ILogger<UpdateProductPricesHostedService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(4));

        await DoWorkAsync(cancellationToken);

        while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
        {
            try
            {
                await DoWorkAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating product prices");
            }
        }
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var extractor = scope.ServiceProvider.GetRequiredService<IExtractor>();

        var products = dbContext.TrackingProducts.ToList();

        if (products.Count == 0)
        {
            _logger.LogInformation("No products found");
            return;
        }

        _logger.LogInformation("Updating product prices");

        foreach (var product in products)
        {
            var price = await extractor.ExtractPrice(product.Url);
            if (price == null)
            {
                _logger.LogWarning("Failed to extract price for product {Id}:{Url}", product.TrackingProductId, product.Url);
                continue;
            }

            dbContext.TrackingProductPrices.Add(new TrackingProductPrice
            {
                Price = price.Value,
                AddedAt = DateTime.UtcNow,
                TrackingProductId = product.TrackingProductId
            });

            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Saved price {Price} for product {Id}:{Url}", price, product.TrackingProductId, product.Url);
        }

        _logger.LogInformation("Updated product prices");
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