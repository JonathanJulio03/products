using System.ComponentModel.DataAnnotations;

namespace Products.Api.Infrastructure.In.Rest.Command;

public record ProductDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; init; }
}