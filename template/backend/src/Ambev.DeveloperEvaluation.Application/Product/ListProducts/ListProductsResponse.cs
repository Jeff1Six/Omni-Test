namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsResponse
{
    public List<object> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
