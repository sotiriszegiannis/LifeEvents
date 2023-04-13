using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Domain
{
    public class AppDbContext : IdentityDbContext
    {
        ITenantResolver TenantResolver { get; set; }
        public DbSet<LifeEvent> LifeEvents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantResolver tenantResolver) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;
            TenantResolver = tenantResolver;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<Tag>().Property(e => e.TenantId).IsRequired();
            modelBuilder.Entity<LifeEvent>().Property(e => e.TenantId).IsRequired();

            modelBuilder.Entity<User>().HasQueryFilter(e => e.TenantId == (TenantResolver != null ? TenantResolver.GetCurrentTenantId() : ""));
            modelBuilder.Entity<Tag>().HasQueryFilter(e => e.TenantId == (TenantResolver != null ? TenantResolver.GetCurrentTenantId() : ""));
            modelBuilder.Entity<LifeEvent>().HasQueryFilter(e => e.TenantId == (TenantResolver != null ? TenantResolver.GetCurrentTenantId() : ""));
        }
    }
}
