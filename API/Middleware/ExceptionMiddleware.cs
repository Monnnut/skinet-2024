using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;
//env checks what kind of environment the system is running in
//requestDelegate returns a finish task

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    //cant use different name as middleware expect to see InvokeAsync
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {

            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        //set response header
        context.Response.ContentType = "application/json";
        //set error number
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        //set reponse depanding in env
        var response = env.IsDevelopment()
        ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
        : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal server error");

        //create json response to be sent back to client
        //set naming policy
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        //convert into json string
        var json = JsonSerializer.Serialize(response, options);
        //write json to the response body to be sent back to client
        return context.Response.WriteAsync(json);
    }
}
