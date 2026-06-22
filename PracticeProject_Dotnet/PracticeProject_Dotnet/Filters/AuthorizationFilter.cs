using Microsoft.AspNetCore.Mvc.Filters;

namespace PracticeProject_Dotnet.Filters;

public class AuthorizationFilter(IConfiguration configuration) : IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //Console.WriteLine(context);
        //Console.WriteLine("Authorization Filter started");
    }
}