using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PracticeProject_Dotnet.Filters;
using PracticeProject_Dotnet.KeyedServiceExample;
using PracticeProject_Dotnet.Patterns;
using PracticeProject_Dotnet.Models;

namespace PracticeProject_Dotnet.Controllers
{
    [Route("/api/v1/ProductsController")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly IProductService productService;
        private readonly IConfiguration configuration;
        private readonly IOptions<OptionsPatternExample> options;
        private readonly IKeyedService keyedService;

        public ProductsController(IProductService productService, IConfiguration configuration, IOptions<OptionsPatternExample> options, IKeyedService keyedService)
        {
            this.productService = productService;
            this.configuration = configuration;
            this.options = options;
            this.keyedService = keyedService;
        }

        [HttpGet("environment")]
        public IActionResult GetEnvDetails()
        {
            return Ok(configuration["SampleKey"]);
        }

        [HttpGet("optionsPattern")]
        public IActionResult GetConfigsFromOptionPattern()
        {
            var result = options.Value;
            return Ok(options.Value);
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        [HttpGet("keyedService/{notificationType}")]
        public IActionResult GetKeyedService(string notificationType)
        {
            return Ok(keyedService.KeyedServiceExample(notificationType));
        }

        [ServiceFilter(typeof(ResourceFilter))]
        [HttpGet("products")]
        public IActionResult GetAllProducts()
        {
            // var products = productService.GetAllAsync().Result;
            // InMemoryCacheDummy.cachedProducts = products;
            return Ok(productService.GetAllAsync().Result);
        }

        [HttpGet("product/{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = productService.GetByIdAsync(id).Result;
            if(product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost("product")]
        public IActionResult CreateNewProduct(CreateProductRequest productRequest)
        {
            throw new Exception();
            var newProduct = productService.CreateAsync(productRequest);

            if(newProduct is null) return NotFound(newProduct);

            return Created("from create",newProduct);
        }

        [HttpPut("product/{id}")]
        public IActionResult UpdateProduct(int id, UpdateProductRequest productRequest)
        {
            var updatedProduct = productService.UpdateAsync(id, productRequest);

            if (updatedProduct is null) return NotFound(updatedProduct);

            return Ok(updatedProduct);
        }

        [HttpPatch("product/{id}")]
        public IActionResult PatchProduct(int id, PatchProductRequest patchProductRequest)
        {
            var patchedProduct = productService.PatchAsync(id, patchProductRequest);

            if(patchedProduct is null) return NotFound(patchedProduct);

            return Ok(patchedProduct);
        }


        [HttpDelete("product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var deletedProduct = productService.DeleteAsync(id);

            if(deletedProduct is null) return NotFound(deletedProduct);

            return Ok(deletedProduct);
        }


        
    }
}
