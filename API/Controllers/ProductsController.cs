using System;
using Core.Entities;
using Core.Interfaces;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
//api controller gives us automatic controller binding
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, 
    string? type, string? sort)
    {


        return Ok(await repo.GetProductAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")] // api/products/12
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await repo.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        //check if our changes is saved
        if (await repo.SaveChangesAsync())
        {
            //Saves the new product to the database.
            //return CreatedAtAction("ActionName", routeValues, value);
            //"ActionName": The name of the action method that retrieves the resource (e.g., GetProduct).
            // routeValues: Route parameters needed to generate the URL (e.g., { id = product.Id }).
            // value: The newly created resource object (e.g., product) returned in the response body.
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem creating product");

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //check if product exist and the product and id align
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Cannot update this product");
        }

        //entry provide access to being track
        //tell them that is is being modfied
        repo.UpdateProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem with updating product");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        //find the product that we want to delete
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();
        //remove product
        repo.DeleteProduct(product);
        //save changes
        if (await repo.SaveChangesAsync())
        {
            //return nothing
            return NoContent();
        }
        return BadRequest("Problem deleting product");

    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }

}
