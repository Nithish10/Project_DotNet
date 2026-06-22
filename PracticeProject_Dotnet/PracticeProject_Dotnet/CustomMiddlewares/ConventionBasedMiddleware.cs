namespace PracticeProject_Dotnet.CustomMiddlewares
{
    public class ConventionBasedMiddleware(RequestDelegate request)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Console.WriteLine("Convention based middlware -- Started");
            //Console.WriteLine($" Request Body {context.Request.Body}");
            await request(context);
            //Console.WriteLine("Convention based middlware -- Ended");
        }
    }
}