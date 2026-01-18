using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Models;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    /// <summary>
    /// Controller for managing product controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ProductsController(IMediator mediator, IMapper mapper, DefaultContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        [FromQuery] string? category = null,
        [FromQuery] string? title = null,
        [FromQuery(Name = "_minPrice")] decimal? minPrice = null,
        [FromQuery(Name = "_maxPrice")] decimal? maxPrice = null
    )
        {
            var result = await _mediator.Send(new ListProductsQuery
            {
                Page = page,
                Size = size,
                Order = order,
                Category = category,
                Title = title,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            });

            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMapper mapper)
        {
            var result = await _mediator.Send(new GetProductQuery(id));

            var response = mapper.Map<GetProductResponse>(result);

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, [FromServices] IMapper mapper)
        {
            var command = mapper.Map<CreateProductCommand>(request);

            var result = await _mediator.Send(command);

            var response = mapper.Map<CreateProductResponse>(result);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CreateProductRequestDto request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound(new { type = "ResourceNotFound", error = "Product not found", detail = $"Product {id} not found" });

            ApplyRequest(product, request);
            await _context.SaveChangesAsync();

            return Ok(ToResponse(product));
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return NotFound(new { type = "ResourceNotFound", error = "Product not found", detail = $"Product {id} not found" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted" });
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(
                string category,
                [FromQuery(Name = "_page")] int page = 1,
                [FromQuery(Name = "_size")] int size = 10,
                [FromQuery(Name = "_order")] string? order = null
            )
        {
            if (page <= 0) page = 1;
            if (size <= 0) size = 10;
            if (size > 100) size = 100;

            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Where(x => x.Category == category);

            query = ApplyOrder(query, order);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var data = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return Ok(new
            {
                data,
                totalItems,
                currentPage = page,
                totalPages
            });
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Products
                .AsNoTracking()
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            return Ok(categories);
        }

        #region Auxiliares
        private static IQueryable<Product> ApplyOrder(IQueryable<Product> query, string? order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return query.OrderBy(x => x.Id);

            // Examples:
            // "price desc, title asc"
            // "price desc, title"
            var parts = order
                .Trim()
                .Trim('"')
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            IOrderedQueryable<Product>? ordered = null;

            foreach (var part in parts)
            {
                var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var field = tokens[0].ToLowerInvariant();
                var direction = tokens.Length > 1 ? tokens[1].ToLowerInvariant() : "asc";

                bool desc = direction == "desc";

                ordered = (field, desc, ordered) switch
                {
                    ("price", true, null) => query.OrderByDescending(x => x.Price),
                    ("price", false, null) => query.OrderBy(x => x.Price),

                    ("title", true, null) => query.OrderByDescending(x => x.Title),
                    ("title", false, null) => query.OrderBy(x => x.Title),

                    ("category", true, null) => query.OrderByDescending(x => x.Category),
                    ("category", false, null) => query.OrderBy(x => x.Category),

                    ("id", true, null) => query.OrderByDescending(x => x.Id),
                    ("id", false, null) => query.OrderBy(x => x.Id),

                    ("price", true, not null) => ordered.ThenByDescending(x => x.Price),
                    ("price", false, not null) => ordered.ThenBy(x => x.Price),

                    ("title", true, not null) => ordered.ThenByDescending(x => x.Title),
                    ("title", false, not null) => ordered.ThenBy(x => x.Title),

                    ("category", true, not null) => ordered.ThenByDescending(x => x.Category),
                    ("category", false, not null) => ordered.ThenBy(x => x.Category),

                    ("id", true, not null) => ordered.ThenByDescending(x => x.Id),
                    ("id", false, not null) => ordered.ThenBy(x => x.Id),

                    _ => ordered ?? query.OrderBy(x => x.Id)
                };
            }

            return ordered ?? query.OrderBy(x => x.Id);
        }
        private static ProductResponseDto ToResponse(Product p)
        {
            return new ProductResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Category = p.Category,
                Image = p.Image,
                Rating = new ProductRatingDto
                {
                    Rate = p.RatingRate,
                    Count = p.RatingCount
                }
            };
        }

        private static void ApplyRequest(Product p, CreateProductRequestDto req)
        {
            p.Title = req.Title;
            p.Price = req.Price;
            p.Description = req.Description;
            p.Category = req.Category;
            p.Image = req.Image;
            p.RatingRate = req.Rating?.Rate ?? 0;
            p.RatingCount = req.Rating?.Count ?? 0;
        }

        #endregion

    }
}