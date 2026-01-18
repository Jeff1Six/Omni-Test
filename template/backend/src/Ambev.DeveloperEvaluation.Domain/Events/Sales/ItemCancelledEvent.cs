namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public sealed record ItemCancelledEvent(
    Guid SaleId,
    Guid SaleItemId,
    Guid ProductId,
    int Quantity,
    DateTime CancelledAt
);
