using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //why read only? not modifying
    Task<IReadOnlyList<Product>> GetProductAsync(string? brand, string? type, string? sort);
    //when getting a single product we know it might be null, use optional operator
    Task<Product?> GetProductByIdAsync(int id);
    //Get async Brand from the data
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();
    //dont need async as they dont interact with datbase
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);

    bool ProductExists(int id);

    Task<bool> SaveChangesAsync();

}
