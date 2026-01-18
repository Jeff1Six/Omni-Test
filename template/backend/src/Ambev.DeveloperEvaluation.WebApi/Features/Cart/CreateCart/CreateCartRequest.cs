using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public sealed class CreateCartRequest
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductRequest> Products { get; set; } = new();

}
