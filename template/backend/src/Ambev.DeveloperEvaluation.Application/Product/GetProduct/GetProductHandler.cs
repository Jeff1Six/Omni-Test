using Ambev.DeveloperEvaluation.Common.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public GetProductHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product {request.Id} not found");

        return _mapper.Map<GetProductResult>(product);
    }
}
