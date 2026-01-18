using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public sealed class GetCartHandler : IRequestHandler<GetCartQuery, GetCartResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public GetCartHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cart is null)
            throw new KeyNotFoundException($"Cart {request.Id} not found");

        return _mapper.Map<GetCartResult>(cart);
    }
}
