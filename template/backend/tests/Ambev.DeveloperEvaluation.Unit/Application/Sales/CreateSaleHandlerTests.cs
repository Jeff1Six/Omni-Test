using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private static IMapper CreateMapper()
    {
        var cfg = new MapperConfiguration(c =>
        {
            c.CreateMap<Sale, CreateSaleResult>();
            c.CreateMap<SaleItem, CreateSaleResult.SaleItemResult>();
        });

        cfg.AssertConfigurationIsValid();
        return cfg.CreateMapper();
    }

    [Fact(DisplayName = "Given valid request When creating sale Then creates sale with calculated totals")]
    public async Task Handle_ValidRequest_CreatesSale()
    {
        // Arrange
        var mapper = CreateMapper();
        var context = DbContextHelper.CreateInMemoryContext();
        var eventBus = Substitute.For<IEventBus>();

        var handler = new CreateSaleHandler(context, mapper, eventBus);
        var command = SaleHandlerTestData.CreateValidCreateCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        var saved = await context.Sales
            .AsNoTracking()
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == result.Id);

        saved.Should().NotBeNull();
        saved!.Items.Should().HaveCount(1);

        saved.TotalAmount.Should().Be(36m); // 4 items => 10% => 40 - 4 = 36

        await eventBus.Received(1).PublishAsync(
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given qty > 20 When creating sale Then throws exception")]
    public async Task Handle_QtyAbove20_Throws()
    {
        // Arrange
        var mapper = CreateMapper();
        var context = DbContextHelper.CreateInMemoryContext();
        var eventBus = Substitute.For<IEventBus>();

        var handler = new CreateSaleHandler(context, mapper, eventBus);

        var command = SaleHandlerTestData.CreateValidCreateCommand();
        command.Items[0].Quantity = 21;

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<System.ComponentModel.DataAnnotations.ValidationException>();

        await eventBus.DidNotReceive().PublishAsync(
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }
}
