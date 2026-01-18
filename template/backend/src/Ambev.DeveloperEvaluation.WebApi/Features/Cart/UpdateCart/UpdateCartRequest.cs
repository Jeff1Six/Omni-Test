using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public sealed class UpdateCartRequest
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }

    public List<CartProductRequest> Products { get; set; } = new();

}
