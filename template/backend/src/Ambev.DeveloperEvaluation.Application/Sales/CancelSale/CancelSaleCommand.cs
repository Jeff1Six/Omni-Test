using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public sealed class CancelSaleCommand : IRequest<CancelSaleResult>
{
    public Guid Id { get; set; }
}
