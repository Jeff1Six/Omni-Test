using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public sealed class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private readonly DefaultContext _context;

    public DeleteCartHandler(DefaultContext context)
    {
        _context = context;
    }

    public async Task<DeleteCartResult> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cart is null)
            throw new KeyNotFoundException($"Cart {request.Id} not found");

        if (cart.Products is not null && cart.Products.Any())
            _context.CartItems.RemoveRange(cart.Products);

        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteCartResult
        {
            Message = "Cart deleted successfully"
        };
    }
}
