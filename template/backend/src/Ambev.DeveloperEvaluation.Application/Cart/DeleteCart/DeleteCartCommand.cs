using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public sealed class DeleteCartCommand : IRequest<DeleteCartResult>
{
    public Guid Id { get; set; }
}
