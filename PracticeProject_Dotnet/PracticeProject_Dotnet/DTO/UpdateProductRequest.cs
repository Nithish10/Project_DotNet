using System.ComponentModel.DataAnnotations;

public record UpdateProductRequest(
    //[property: Required, StringLength(200)] string Name,
    [Required, StringLength(100)] string Name,
    [Required] string Description,
    [Range(0.01, 1000000)] decimal Price,
    [Range(0, int.MaxValue)] int Stock,
    string? Category
);