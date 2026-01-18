namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public sealed class ListSalesResult
{
    public List<SaleSummaryResult> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public sealed class SaleSummaryResult
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
