namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public sealed class GetSaleResponse
{
    public Guid Id { get; set; }
    public int SaleNumber { get; set; }
    public DateTime Date { get; set; }
}
