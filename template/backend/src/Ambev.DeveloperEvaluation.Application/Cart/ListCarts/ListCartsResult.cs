namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public sealed class ListCartsResult
{
    public List<CartSummaryResult> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public sealed class CartSummaryResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public List<CartProductResult> Products { get; set; } = new();

        public sealed class CartProductResult
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
