namespace PracticeProject_Dotnet.CustomMiddlewares
{
    public class CorrelationBasedMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //Console.WriteLine("This is from correlation middleware -- Started");
            await next(context);
            //Console.WriteLine("This is from correlation middleware -- Ended");
        }
    }
}