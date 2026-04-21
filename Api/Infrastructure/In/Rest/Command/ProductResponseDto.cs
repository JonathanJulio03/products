namespace Products.Api.Infrastructure.In.Rest.Command;

public record ProductResponseDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedDate
);