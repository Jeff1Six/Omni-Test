namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public sealed record SaleCreatedEvent(
    Guid SaleId,
    int SaleNumber,
    DateTime Date,
    Guid CustomerId,
    Guid BranchId,
    decimal TotalAmount
);
