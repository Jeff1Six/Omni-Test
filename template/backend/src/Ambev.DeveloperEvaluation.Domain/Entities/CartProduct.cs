using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartProduct : BaseEntity
{
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
