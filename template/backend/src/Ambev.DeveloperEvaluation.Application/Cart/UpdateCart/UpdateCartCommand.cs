using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public sealed class UpdateCartCommand : IRequest<UpdateCartResult>
{
    public Guid Id { get; set; } 

    public Guid UserId { get; init; }
    public DateTime Date { get; init; }
    public List<CartProductCommand> Products { get; init; } = new();

    public sealed class CartProductCommand
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
