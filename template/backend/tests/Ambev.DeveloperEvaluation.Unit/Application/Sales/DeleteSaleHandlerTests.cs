using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class DeleteSaleHandlerTests
{
    [Fact(DisplayName = "Given existing sale When deleting Then marks as cancelled")]
    public async Task Handle_SaleExists_CancelsSale()
    {
        // Arrange
        var mapper = Substitute.For<IMapper>();
        var context = DbContextHelper.CreateInMemoryContext();
        var eventBus = Substitute.For<IEventBus>();

        var sale = SaleHandlerTestData.CreateSaleEntity();
        context.Sales.Add(sale);
        await context.SaveChangesAsync();

        var handler = new CancelSaleHandler(context, mapper, eventBus);
        var command = SaleHandlerTestData.CreateDeleteCommand(sale.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var updated = await context.Sales
            .AsNoTracking()
            .FirstAsync(s => s.Id == sale.Id);

        updated.Cancelled.Should().BeTrue();

        await eventBus.Received(1).PublishAsync(
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non existing sale When deleting Then throws")]
    public async Task Handle_SaleNotFound_Throws()
    {
        // Arrange
        var mapper = Substitute.For<IMapper>();
        var context = DbContextHelper.CreateInMemoryContext();
        var eventBus = Substitute.For<IEventBus>();

        var handler = new CancelSaleHandler(context, mapper, eventBus);
        var command = SaleHandlerTestData.CreateDeleteCommand(Guid.NewGuid());

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();

        await eventBus.DidNotReceive().PublishAsync(
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }
}
