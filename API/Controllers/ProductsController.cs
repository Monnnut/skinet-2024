using System;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;
//api controller gives us automatic controller binding

public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams)
    {

        var spec = new ProductSpecification(specParams);


        return Ok(await CreatePageResult(repo, spec, specParams.PageIndex, specParams.PageSize));
    }

    [HttpGet("{id:int}")] // api/products/12
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await repo.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        //check if our changes is saved
        if (await repo.SaveAllAsync())
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
        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem with updating product");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        //find the product that we want to delete
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();
        //remove product
        repo.Remove(product);
        //save changes
        if (await repo.SaveAllAsync())
        {
            //return nothing
            return NoContent();
        }
        return BadRequest("Problem deleting product");

    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }
    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }

}
