using System;
using System.Text.Json;
using Core.Entities;

namespace infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        //check if there is any products
        if (!context.Products.Any())
        {
            //read file from seed data using ReadALLTextAsync
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            //change json fomat to list with JsonSerializer to object
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            //DeSerializer can be null
            if (products == null)
            {
                return;
            }
            //start tracking entities so that can be added to database with add range
            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
