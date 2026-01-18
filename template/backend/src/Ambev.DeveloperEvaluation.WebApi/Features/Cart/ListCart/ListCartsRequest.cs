namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public sealed class ListCartsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
}
