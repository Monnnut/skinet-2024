using System;
using Core.Entities;
using infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;
//get nuget. SQL+design(for migration)
//a class that establish connection with data
//dont forget to add service for this to work
public class StoreContext(DbContextOptions options) : DbContext(options)
{
    //use product entities to create tables
    public DbSet<Product>? Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //  apply all entity configurations from a specific assembly
        //Essentially, it tells Entity Framework Core to look for all configuration classes in the same assembly as ProductConfiguration
        //typeof ensure EF knows exactly where to look for
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }


}
