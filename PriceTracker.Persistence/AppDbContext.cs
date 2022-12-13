using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PriceTracker.Entities;
using PriceTracker.Entities.Providers;

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

    public IEnumerable<(object Entity, IValidator Validator)> GetValidatableEntities()
    {
        var validatorProvider = this.GetService<IValidatorProvider>();
        var serviceProvider = this.GetService<IServiceProvider>();

        var entities = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity);

        var validators = entities.Select(entity =>
            (Entity: entity,
                Validator: validatorProvider.GetValidator(entity.GetType(), serviceProvider)));

        return validators.Where(v => v.Validator != null);
    }


    private async Task ValidateChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var (entry, validator) in GetValidatableEntities())
        {
            var result = await validator.ValidateAsync(new ValidationContext<object?>(Convert.ChangeType(entry, entry.GetType())), cancellationToken);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }

    private void ValidateChanges()
    {
        foreach (var (entry, validator) in GetValidatableEntities())
        {
            var result = validator.Validate(new ValidationContext<object?>(Convert.ChangeType(entry, entry.GetType())));
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await ValidateChangesAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        await ValidateChangesAsync(cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        ValidateChanges();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ValidateChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //         optionsBuilder.UseSqlite("Data Source=app.db", options => options.MigrationsAssembly("PriceTracker.Persistence"));
    // }

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