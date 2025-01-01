using System;
using API.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        //check to see if a user is authorize
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Not a good request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
       return NotFound();
    }
    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        //get any server error
        throw new Exception("This is a test exception");
    }
    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        //send empty body
        //need dto. Transfer data between layers or network
        return Ok();
    }
}
