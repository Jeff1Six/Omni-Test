using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public sealed class ListCartsQuery : IRequest<ListCartsResult>
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 10;
    public string? Order { get; init; }
}
