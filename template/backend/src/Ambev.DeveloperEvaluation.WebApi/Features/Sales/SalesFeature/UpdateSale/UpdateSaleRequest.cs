namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public sealed class UpdateSaleRequest
{
    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }

    public List<SaleItemRequest> Items { get; set; } = new();

    public sealed class SaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
