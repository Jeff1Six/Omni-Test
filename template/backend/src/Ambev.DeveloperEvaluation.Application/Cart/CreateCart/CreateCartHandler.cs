using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public CreateCartHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateCartResult> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new KeyNotFoundException($"User {request.UserId} not found");

        var productIds = request.Products.Select(x => x.ProductId).Distinct().ToList();

        var existingProductIds = await _context.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var missing = productIds.Except(existingProductIds).ToList();
        if (missing.Any())
            throw new KeyNotFoundException($"Products not found: {string.Join(", ", missing)}");

        var cart = new Cart
        {
            UserId = request.UserId,
            Date = request.Date,
            Products = request.Products.Select(p => new CartItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreateCartResult>(cart);
    }
}
