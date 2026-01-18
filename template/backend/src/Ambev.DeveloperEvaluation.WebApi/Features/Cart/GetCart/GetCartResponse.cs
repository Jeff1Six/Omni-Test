namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public sealed class GetCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }

    public List<CartProductResponse> Products { get; set; } = new();

    public sealed class CartProductResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
