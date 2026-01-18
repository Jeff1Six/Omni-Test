namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public sealed class CartProductRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
