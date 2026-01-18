using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleHandlerTests
{
    private static IMapper CreateMapper()
    {
        var cfg = new MapperConfiguration(c =>
        {
            c.CreateMap<Sale, GetSaleResult>();
            c.CreateMap<SaleItem, CreateSaleResult.SaleItemResult>();
        });

        cfg.AssertConfigurationIsValid();
        return cfg.CreateMapper();
    }

    [Fact(DisplayName = "Given existing sale When getting by id Then returns sale")]
    public async Task Handle_SaleExists_ReturnsSale()
    {
        // Arrange
        var mapper = CreateMapper();
        var context = DbContextHelper.CreateInMemoryContext();

        var sale = SaleHandlerTestData.CreateSaleEntity();
        sale.Items.Add(SaleHandlerTestData.CreateActiveItem(
            sale.Id,
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            4,
            10m));

        sale.TotalAmount = sale.Items.Where(i => !i.Cancelled).Sum(i => i.TotalItemAmount);

        context.Sales.Add(sale);
        await context.SaveChangesAsync();

        var handler = new GetSaleHandler(context, mapper);
        var query = SaleHandlerTestData.CreateGetByIdQuery(sale.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
    }

    [Fact(DisplayName = "Given non existing sale When getting by id Then throws")]
    public async Task Handle_SaleNotFound_Throws()
    {
        // Arrange
        var mapper = CreateMapper();
        var context = DbContextHelper.CreateInMemoryContext();

        var handler = new GetSaleHandler(context, mapper);
        var query = SaleHandlerTestData.CreateGetByIdQuery(Guid.NewGuid());

        // Act
        var act = () => handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
