using Microsoft.EntityFrameworkCore;
using PriceTracker.Entities;

namespace PriceTracker.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ConsumedRefreshToken> ConsumedRefreshTokens { get; set; }
    public DbSet<TrackingProduct> TrackingProducts { get; set; }
    public DbSet<TrackingProductPrice> TrackingProductPrices { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=app.db", options => options.MigrationsAssembly("PriceTracker.Persistence"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.EMail).IsUnique();
        });

        builder.Entity<ConsumedRefreshToken>()
            .HasIndex(c => c.ConsumedRefreshTokenId)
            .IsUnique();

        builder.Entity<TrackingProduct>()
            .HasIndex(c => c.Url)
            .IsUnique();
    }
}