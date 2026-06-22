public class ProductService : IProductService
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 50, Category = "Electronics" },
        new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Stock = 200, Category = "Electronics" },
        new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, Stock = 100, Category = "Electronics" }
    };


    public Task<Product> CreateAsync(CreateProductRequest request)
    {
        var newProductId = _products.Count + 1;

        if(request is null || request.Name.Length == 0 || request.Description.Length == 0)
        {
            return null;
        }

        var product = new Product()
        {
          Id=newProductId,
          Name=request.Name,
          Description=request.Description,
          Price=request.Price,
          Stock=request.Stock,
          Category = request.Category ?? "Electronics",
        };

        _products.Add(product);


        return Task.FromResult(product);


    }

    public Task<bool> DeleteAsync(int id)
    {
        var productToDelete = _products.FirstOrDefault(product => product.Id == id);

        if(productToDelete is null) return Task.FromResult(false);

        _products.Remove(productToDelete);

        return Task.FromResult(true);
        
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(product => product.Id == id);

        return Task.FromResult<Product>(product);
    }

    public Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var products = _products.Skip((page -1) * pageSize).Take(pageSize);

        return Task.FromResult<(IEnumerable<Product>,int)>((products, _products.Count));
    }

    public Task<Product?> PatchAsync(int id, PatchProductRequest request)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null) return Task.FromResult<Product?>(null);

        if (request.Name is not null) product.Name = request.Name;
        if (request.Description is not null) product.Description = request.Description;
        if (request.Price.HasValue) product.Price = request.Price.Value;
        if (request.Stock.HasValue) product.Stock = request.Stock.Value;
        if (request.Category is not null) product.Category = request.Category;
        product.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult<Product?>(product);

    }

    public Task<Product?> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null) return Task.FromResult<Product?>(null);

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Category = request.Category;
        product.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult<Product?>(product);
    }
}