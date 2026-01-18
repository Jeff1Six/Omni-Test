using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public sealed class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public GetSaleHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException($"Sale {request.Id} not found");

        return _mapper.Map<GetSaleResult>(sale);
    }
}
