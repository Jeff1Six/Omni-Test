public sealed class CreateCartResult
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
