using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public sealed class ListCartsHandler : IRequestHandler<ListCartsQuery, ListCartsResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public ListCartsHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListCartsResult> Handle(ListCartsQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Carts
            .AsNoTracking()
            .Include(x => x.Products)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Order))
        {
            var order = request.Order.Trim().ToLower();

            query = order switch
            {
                "id asc" => query.OrderBy(x => x.Id),
                "id desc" => query.OrderByDescending(x => x.Id),

                "userid asc" => query.OrderBy(x => x.UserId),
                "userid desc" => query.OrderByDescending(x => x.UserId),

                "date asc" => query.OrderBy(x => x.Date),
                "date desc" => query.OrderByDescending(x => x.Date),

                _ => query.OrderBy(x => x.Id)
            };
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);

        var carts = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new ListCartsResult
        {
            Data = _mapper.Map<List<ListCartsResult.CartSummaryResult>>(carts),
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }

}
