namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsRequest
{
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;
    public string? _order { get; set; }

    public string? category { get; set; }
    public string? title { get; set; }
    public decimal? _minPrice { get; set; }
    public decimal? _maxPrice { get; set; }
}
