namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = default!;

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
