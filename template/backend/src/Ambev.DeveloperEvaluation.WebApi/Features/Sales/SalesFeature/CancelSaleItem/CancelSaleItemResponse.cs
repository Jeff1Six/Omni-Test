namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

public sealed class CancelSaleItemResponse
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public bool Cancelled { get; set; }
    public decimal SaleTotalAmount { get; set; }
}
