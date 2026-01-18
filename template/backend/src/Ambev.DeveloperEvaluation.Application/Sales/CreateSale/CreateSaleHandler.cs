using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CreateSaleHandler(DefaultContext context, IMapper mapper, IEventBus eventBus)
    {
        _context = context;
        _mapper = mapper;
        _eventBus = eventBus;

    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var lastNumber = await _context.Sales
            .AsNoTracking()
            .MaxAsync(x => (int?)x.SaleNumber, cancellationToken);

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = (lastNumber ?? 0) + 1,
            Date = request.Date,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Cancelled = false,
            Items = new List<SaleItem>()
        };

        foreach (var item in request.Items)
        {
            var discountPercentage = SalesRulesService.GetDiscountPercentage(item.Quantity);

            var discountAmount = item.UnitPrice * item.Quantity * discountPercentage;
            var totalItemAmount = (item.UnitPrice * item.Quantity) - discountAmount;

            sale.Items.Add(new SaleItem
            {
                Id = Guid.NewGuid(),
                SaleId = sale.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = discountAmount,
                TotalItemAmount = totalItemAmount,
                Cancelled = false
            });
        }

        sale.TotalAmount = sale.Items.Sum(x => x.TotalItemAmount);

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);


        await _eventBus.PublishAsync(new SaleCreatedEvent(
            sale.Id,
            sale.SaleNumber,
            sale.Date,
            sale.CustomerId,
            sale.BranchId,
            sale.TotalAmount
        ), cancellationToken);


        return _mapper.Map<CreateSaleResult>(sale);
    }
}
