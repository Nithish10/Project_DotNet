using Microsoft.AspNetCore.Mvc;

namespace PracticeProject_Dotnet.Exceptions;

public class CustomExceptionHandler(RequestDelegate request)
{
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch(Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    public static async Task HandleAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "An error occurred",
            Detail = ex.Message
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}