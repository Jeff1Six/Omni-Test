namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public sealed class CancelSaleItemResult
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public bool Cancelled { get; set; }
    public decimal SaleTotalAmount { get; set; }
}
