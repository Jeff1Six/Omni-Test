using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public sealed class GetSaleQuery : IRequest<GetSaleResult>
{
    public Guid Id { get; init; }
}
