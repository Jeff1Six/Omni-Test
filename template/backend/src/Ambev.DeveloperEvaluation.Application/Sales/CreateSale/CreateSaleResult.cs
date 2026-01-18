namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed class CreateSaleResult
{
    public Guid Id { get; set; }
    public int SaleNumber { get; set; }

    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }

    public List<SaleItemResult> Items { get; set; } = new();

    public sealed class SaleItemResult
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal TotalItemAmount { get; set; }

        public bool Cancelled { get; set; }
    }
}
