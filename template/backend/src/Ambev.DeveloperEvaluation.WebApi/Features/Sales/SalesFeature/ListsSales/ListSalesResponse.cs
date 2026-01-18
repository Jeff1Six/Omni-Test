namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public sealed class ListSalesResponse
{
    public List<SaleResponseItem> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public sealed class SaleResponseItem
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Cancelled { get; set; }
    }
}
