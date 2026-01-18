namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SalesFeature.SaleCreate;

public sealed class CreateSaleResponse
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

    public List<SaleItemResponse> Items { get; set; } = new();

    public sealed class SaleItemResponse
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
