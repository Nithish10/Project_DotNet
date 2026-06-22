namespace PracticeProject_Dotnet.CustomMiddlewares;

public static class ExtensionBasedMiddleware
{
    public static IApplicationBuilder UseMiddlewareExtensionMethod(this IApplicationBuilder app)
    {
        //Console.WriteLine("This is from extension middleware --  started");
        return app.UseMiddleware<ConventionBasedMiddleware>();        
    }
}