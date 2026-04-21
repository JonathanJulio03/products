namespace Products.Api.Domain;

public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalRecords,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}