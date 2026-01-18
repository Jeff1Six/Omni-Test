using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleItemHandlerTests
{
    [Fact(DisplayName = "Given existing sale item When cancelling Then marks item as cancelled and recalculates total")]
    public async Task Handle_ItemExists_CancelsItem()
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryContext();
        var eventBus = Substitute.For<IEventBus>();
        var mapper = Substitute.For<IMapper>();

        var sale = SaleHandlerTestData.CreateSaleEntity();

        var item1 = SaleHandlerTestData.CreateActiveItem(
            sale.Id,
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            4,
            10m);

        var item2 = SaleHandlerTestData.CreateActiveItem(
            sale.Id,
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            4,
            10m);

        sale.Items.Add(item1);
        sale.Items.Add(item2);
        sale.TotalAmount = sale.Items.Where(i => !i.Cancelled).Sum(i => i.TotalItemAmount);

        context.Sales.Add(sale);
        await context.SaveChangesAsync();

        var handler = new CancelSaleItemHandler(context, mapper, eventBus);

        var command = new CancelSaleItemCommand
        {
            SaleId = sale.Id,
            ItemId = item1.Id
        };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var updated = await context.Sales
            .AsNoTracking()
            .Include(s => s.Items)
            .FirstAsync(s => s.Id == sale.Id);

        updated.Items.Single(i => i.Id == item1.Id).Cancelled.Should().BeTrue();
        updated.Items.Single(i => i.Id == item2.Id).Cancelled.Should().BeFalse();

        updated.TotalAmount.Should().Be(item2.TotalItemAmount);

        await eventBus.Received(1).PublishAsync(
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }
}
