using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed class CreateSaleCommand : IRequest<CreateSaleResult>
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public List<CreateSaleItemCommand> Items { get; set; } = new();

    public sealed class CreateSaleItemCommand
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
