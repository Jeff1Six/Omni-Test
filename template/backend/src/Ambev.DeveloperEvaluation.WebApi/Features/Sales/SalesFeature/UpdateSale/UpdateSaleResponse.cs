namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public sealed class UpdateSaleResponse
{
    public Guid Id { get; set; }

    public int SaleNumber { get; set; }
    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }

    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }

    public List<SaleItemResponse> Items { get; set; } = new();

    public sealed class SaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }

        public bool Cancelled { get; set; }
    }
}
