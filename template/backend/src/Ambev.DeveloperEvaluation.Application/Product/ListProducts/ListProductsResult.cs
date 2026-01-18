using Ambev.DeveloperEvaluation.Application.Products.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsResult
{
    public List<ProductResult> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
