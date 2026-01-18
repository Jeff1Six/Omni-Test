using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public sealed class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }

    public List<SaleItemCommand> Items { get; set; } = new();

    public sealed class SaleItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
