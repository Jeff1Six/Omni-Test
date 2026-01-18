using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public sealed class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public UpdateSaleHandler(DefaultContext context, IMapper mapper, IEventBus eventBus)
    {
        _context = context;
        _mapper = mapper;
        _eventBus = eventBus;

    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException($"Sale {request.Id} not found");

        sale.Date = request.Date;
        sale.CustomerId = request.CustomerId;
        sale.BranchId = request.BranchId;

        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);

        var dbItems = await _context.SaleItems
            .Where(x => x.SaleId == request.Id)
            .ToListAsync(cancellationToken);

        var requestedItems = request.Items
            .GroupBy(x => x.ProductId)
            .Select(g =>
            {
                var qty = g.Sum(x => x.Quantity);
                var unitPrice = g.First().UnitPrice;

                SalesRulesService.GetDiscountPercentage(qty);

                return new
                {
                    ProductId = g.Key,
                    Quantity = qty,
                    UnitPrice = unitPrice
                };
            })
            .ToList();

        var requestedProductIds = requestedItems.Select(x => x.ProductId).ToHashSet();

        foreach (var old in dbItems.Where(x => !x.Cancelled && !requestedProductIds.Contains(x.ProductId)))
        {
            old.Cancelled = true;
            _context.SaleItems.Update(old);

        }

        foreach (var req in requestedItems)
        {
            var existing = dbItems.FirstOrDefault(x => x.ProductId == req.ProductId && !x.Cancelled);

            var discountPercent = SalesRulesService.GetDiscountPercentage(req.Quantity);
            var discountAmount = (req.UnitPrice * req.Quantity) * discountPercent;
            var totalItemAmount = (req.UnitPrice * req.Quantity) - discountAmount;

            if (existing is not null)
            {
                existing.Quantity = req.Quantity;
                existing.UnitPrice = req.UnitPrice;
                existing.DiscountPercent = discountPercent;
                existing.DiscountAmount = discountAmount;
                existing.TotalItemAmount = totalItemAmount;

                _context.SaleItems.Update(existing);
            }
            else
            {
                var newItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    SaleId = sale.Id,
                    ProductId = req.ProductId,
                    Quantity = req.Quantity,
                    UnitPrice = req.UnitPrice,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    TotalItemAmount = totalItemAmount,
                    Cancelled = false
                };

                await _context.SaleItems.AddAsync(newItem, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var activeItems = await _context.SaleItems
            .Where(x => x.SaleId == request.Id && !x.Cancelled)
            .ToListAsync(cancellationToken);

        sale.TotalAmount = activeItems.Sum(x => x.TotalItemAmount);
        _context.Sales.Update(sale);

        await _context.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new SaleModifiedEvent(
            sale.Id,
            sale.SaleNumber,
            sale.Date,
            sale.CustomerId,
            sale.BranchId,
            sale.TotalAmount
        ), cancellationToken);


        var updatedSale = await _context.Sales
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}
