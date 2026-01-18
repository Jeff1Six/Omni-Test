using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public sealed class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public UpdateCartHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cart is null)
            throw new KeyNotFoundException($"Cart {request.Id} not found");

        cart.UserId = request.UserId;
        cart.Date = request.Date;

        _context.Carts.Update(cart);

        var oldItems = await _context.CartItems
            .Where(x => x.CartId == request.Id)
            .ToListAsync(cancellationToken);

        _context.CartItems.RemoveRange(oldItems);

        var newItems = request.Products.Select(p => new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = request.Id,
            ProductId = p.ProductId,
            Quantity = p.Quantity
        }).ToList();

        await _context.CartItems.AddRangeAsync(newItems, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var updatedCart = await _context.Carts
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        return _mapper.Map<UpdateCartResult>(updatedCart);
    }


}
