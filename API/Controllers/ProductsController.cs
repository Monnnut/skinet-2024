using System;
using Core.Entities;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
//api controller gives us automatic controller binding
[ApiController]
[Route("api/[controller]")]
public class ProductsController(StoreContext storeContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {

        var products = await storeContext.Products!.ToListAsync();

        return products;
    }

    [HttpGet("{id:int}")] // api/products/12
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await storeContext.Products!.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        storeContext.Products!.Add(product);

        await storeContext.SaveChangesAsync();

        return product;

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
        storeContext.Entry(product).State = EntityState.Modified;

        await storeContext.SaveChangesAsync();

        //when we are not returning anything
        return NoContent();

    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        //find the product that we want to delete
        var product = await storeContext.Products!.FindAsync(id);

        if (product == null) return NotFound();
        //remove product
        storeContext.Products.Remove(product);
        //save changes
        await storeContext.SaveChangesAsync();
        //return nothing
        return NoContent();
    }
    private bool ProductExists(int id)
    {

        //Any determines element statisfied the condition
        return storeContext.Products!.Any(x => x.Id == id);
    }

}
