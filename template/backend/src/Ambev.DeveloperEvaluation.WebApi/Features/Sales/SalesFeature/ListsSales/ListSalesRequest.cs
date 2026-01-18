namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public sealed class ListSalesRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public bool? Cancelled { get; set; } 
}
