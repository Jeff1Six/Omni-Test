namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; } 

    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = default!;

    // External Identities
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    // % discount (0, 10, 20)
    public decimal DiscountPercent { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalItemAmount { get; set; }

    public bool Cancelled { get; set; }
}
