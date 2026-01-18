using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public sealed class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CancelSaleItemHandler(DefaultContext context, IMapper mapper, IEventBus eventBus)
    {
        _context = context;
        _mapper = mapper;
        _eventBus = eventBus;

    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.SaleId, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException($"Sale {request.SaleId} not found");

        var item = sale.Items.FirstOrDefault(x => x.Id == request.ItemId);

        if (item is null)
            throw new KeyNotFoundException($"SaleItem {request.ItemId} not found in Sale {request.SaleId}");

        if (item.Cancelled)
        {
            return new CancelSaleItemResult
            {
                SaleId = sale.Id,
                ItemId = item.Id,
                Cancelled = true,
                SaleTotalAmount = sale.TotalAmount
            };
        }

        item.Cancelled = true;

        sale.TotalAmount = sale.Items
            .Where(x => !x.Cancelled)
            .Sum(x => x.TotalItemAmount);

        await _context.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new ItemCancelledEvent(
            sale.Id,
            item.Id,
            item.ProductId,
            item.Quantity,
            DateTime.UtcNow
        ), cancellationToken);

        return new CancelSaleItemResult
        {
            SaleId = sale.Id,
            ItemId = item.Id,
            Cancelled = item.Cancelled,
            SaleTotalAmount = sale.TotalAmount
        };
    }
}
