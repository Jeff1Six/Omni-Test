using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public sealed class ListSalesHandler : IRequestHandler<ListSalesQuery, ListSalesResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public ListSalesHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListSalesResult> Handle(ListSalesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Sales
            .AsNoTracking()
            .AsQueryable();

        if (request.Cancelled.HasValue)
        {
            query = query.Where(x => x.Cancelled == request.Cancelled.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Order))
        {
            var order = request.Order.Trim().ToLower();

            query = order switch
            {
                "id asc" => query.OrderBy(x => x.Id),
                "id desc" => query.OrderByDescending(x => x.Id),

                "number asc" => query.OrderBy(x => x.SaleNumber),
                "number desc" => query.OrderByDescending(x => x.SaleNumber),

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

        var sales = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new ListSalesResult
        {
            Data = _mapper.Map<List<ListSalesResult.SaleSummaryResult>>(sales),
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }
}
