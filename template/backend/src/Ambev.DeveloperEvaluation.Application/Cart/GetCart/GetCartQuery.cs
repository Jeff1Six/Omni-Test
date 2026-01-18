using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public sealed class GetCartQuery : IRequest<GetCartResult>
{
    public Guid Id { get; init; }
}
