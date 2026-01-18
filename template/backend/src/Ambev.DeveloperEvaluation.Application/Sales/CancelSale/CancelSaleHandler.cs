using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CancelSaleHandler(DefaultContext context, IMapper mapper, IEventBus eventBus)
    {
        _context = context;
        _mapper = mapper;
        _eventBus = eventBus;

    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException($"Sale {request.Id} not found");

        sale.Cancelled = true;

        foreach (var item in sale.Items)
            item.Cancelled = true;

        await _context.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new SaleCancelledEvent(
            sale.Id,
            sale.SaleNumber,
            DateTime.UtcNow
        ), cancellationToken);

        return _mapper.Map<CancelSaleResult>(sale);
    }
}
