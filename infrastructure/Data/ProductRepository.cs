using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(x => x.Brand)
        .Distinct()
        .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetProductAsync(string? brand, string? type, string? sort)
    {
        //turn data in queryable to get the parameter we want
        var query = context.Products.AsQueryable();
        //check whether paramter is empty or not
        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(x => x.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.Type == type);
        }


            query = sort switch
            {
                "priceAsc"=> query.OrderBy(x => x.Price),
                "priceDesc"=> query.OrderByDescending(x=>x.Price),
                _ => query.OrderBy(x=>x.Name)
            };

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(x => x.Type)
        .Distinct()
        .ToListAsync();

    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        //return int the number of changes that happen in the databse
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)

    {
        //entry provide access to being track
        //tell them that is is being modfied
        context.Entry(product).State = EntityState.Modified;
    }
}
