using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public sealed class CreateCartCommand : IRequest<CreateCartResult>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductCommand> Products { get; set; } = new();

    public sealed class CartProductCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
