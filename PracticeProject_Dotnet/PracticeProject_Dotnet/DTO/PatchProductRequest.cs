public record PatchProductRequest(
    string? Name,
    string? Description,
    decimal? Price,
    int? Stock,
    string? Category
);