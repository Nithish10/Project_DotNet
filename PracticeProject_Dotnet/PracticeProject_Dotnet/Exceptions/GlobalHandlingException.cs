using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PracticeProject_Dotnet.Exceptions;

public class GlobalHandlingException(IProblemDetailsService service) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

         var (status, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

       var problemDetails = new ProblemDetails
       {
           Status = status,
           Title = title,

       } ;

       await service.WriteAsync(new ProblemDetailsContext
       {
           HttpContext = httpContext,
           ProblemDetails = problemDetails
       });

       return true;
    }
}