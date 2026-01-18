using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public int SaleNumber { get; set; } // sequencial

    public DateTime Date { get; set; } = DateTime.UtcNow;

    // External Identities (DDD)
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public bool Cancelled { get; set; }

    public decimal TotalAmount { get; set; }

    public List<SaleItem> Items { get; set; } = new();
}
