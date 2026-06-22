using System.ComponentModel.DataAnnotations;

public record CreateProductRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public string? Category { get; init; }
}