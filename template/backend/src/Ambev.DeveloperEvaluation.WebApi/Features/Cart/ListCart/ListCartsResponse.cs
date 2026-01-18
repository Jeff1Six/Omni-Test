namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public sealed class ListCartsResponse
{
    public List<CartResponseItem> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public sealed class CartResponseItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public List<CartProductResponse> Products { get; set; } = new();

        public sealed class CartProductResponse
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
