using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public sealed class ListSalesQuery : IRequest<ListSalesResult>
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 10;
    public string? Order { get; init; }
    public bool? Cancelled { get; set; } 
}
