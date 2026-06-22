namespace PracticeProject_Dotnet.Services;

public static class InMemoryCacheDummy
{
    public static IEnumerable<Product> cachedProducts =  new List<Product>();
}