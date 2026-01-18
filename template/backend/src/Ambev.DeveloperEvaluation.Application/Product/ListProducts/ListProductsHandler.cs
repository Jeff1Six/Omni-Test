using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsQuery, ListProductsResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public ListProductsHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListProductsResult> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var size = request.Size <= 0 ? 10 : request.Size;
        if (size > 100) size = 100;

        IQueryable<Product> query = _context.Products.AsNoTracking();

        // Filters
        if (!string.IsNullOrWhiteSpace(request.Category))
            query = query.Where(x => x.Category == request.Category);

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var value = request.Title.Trim();
            var starts = value.StartsWith("*");
            var ends = value.EndsWith("*");
            var raw = value.Trim('*');

            if (!string.IsNullOrWhiteSpace(raw))
            {
                if (starts && ends) query = query.Where(x => x.Title.Contains(raw));
                else if (ends) query = query.Where(x => x.Title.StartsWith(raw));
                else if (starts) query = query.Where(x => x.Title.EndsWith(raw));
                else query = query.Where(x => x.Title == raw);
            }
        }

        if (request.MinPrice.HasValue)
            query = query.Where(x => x.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(x => x.Price <= request.MaxPrice.Value);

        // Order
        query = ApplyOrder(query, request.Order);

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new ListProductsResult
        {
            Data = _mapper.Map<List<ProductResult>>(products),
            TotalItems = totalItems,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }

    private static IQueryable<Product> ApplyOrder(IQueryable<Product> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query.OrderBy(x => x.Id);

        var parts = order.Trim().Trim('"')
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
}
