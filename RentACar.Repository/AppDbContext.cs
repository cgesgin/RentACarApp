using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Address>? Addresses { get; set; }
        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Car>? Cars { get; set; }
        public DbSet<CarDetails>? CarDetails { get; set; }
        public DbSet<CarType>? CarTypes { get; set; }
        public DbSet<City>? Cities { get; set; }
        public DbSet<Costumer>? Costumers { get; set; }
        public DbSet<District>? Districts { get; set; }
        public DbSet<Model>? Models { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Rental>? Rentals { get; set; }
        public DbSet<RentalStore>? RentalStores { get; set; }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.IsDeleted = false;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.IsDeleted).IsModified = false;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.IsDeleted = false;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.IsDeleted).IsModified = false;
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}