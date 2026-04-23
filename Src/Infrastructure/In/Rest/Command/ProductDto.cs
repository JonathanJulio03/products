using System.ComponentModel.DataAnnotations;

namespace Products.Src.Infrastructure.In.Rest.Command;

public record ProductDto
{
    private const string SafeInputRegex = @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s.,#\-_!()]+$";
    private const string ErrorMessage = "The field {0} contains invalid characters.";

    [Required]
    [MaxLength(150)]
    [RegularExpression(SafeInputRegex, ErrorMessage = ErrorMessage)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    [RegularExpression(SafeInputRegex, ErrorMessage = ErrorMessage)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [Range(0.01, (double)decimal.MaxValue)]
    public decimal Price { get; init; }
}