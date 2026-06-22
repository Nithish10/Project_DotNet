using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticeProject_Dotnet.Services;

namespace PracticeProject_Dotnet.Filters;

public class ResourceFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        Console.WriteLine($"the resource filter started");

        var stopwatch = Stopwatch.StartNew();

        if(InMemoryCacheDummy.cachedProducts.Count() > 0)
        {
            Console.WriteLine($"the resource filter started -- cache is found");
            context.Result = new OkObjectResult(InMemoryCacheDummy.cachedProducts);
        }
        else
        {
            Console.WriteLine($"the resource filter started -- cache not found");
            var result = await next();

            if(result.Result is OkObjectResult {Value: not null} objectResult)
            {
                Console.WriteLine($"the resource filter started -- cache assigned");
                InMemoryCacheDummy.cachedProducts = (IEnumerable<Product>)objectResult.Value;
            }
        }

        stopwatch.Stop();

        var timeTookToComplete = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"the resource filter ended -- with time {timeTookToComplete}");
    }
}