namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public sealed class UpdateSaleResult
{
    public Guid Id { get; set; }
    public int SaleNumber { get; set; }
    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }

    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }

    public List<SaleItemResult> Items { get; set; } = new();

    public sealed class SaleItemResult
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }

        public bool Cancelled { get; set; }
    }
}
