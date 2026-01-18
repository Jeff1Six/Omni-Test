using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class ListSalesHandlerTests
{
    private static IMapper CreateMapper()
    {
        var cfg = new MapperConfiguration(c =>
        {
            c.CreateMap<Sale, ListSalesResult.SaleSummaryResult>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.SaleNumber));
        });

        cfg.AssertConfigurationIsValid();
        return cfg.CreateMapper();
    }

    [Fact(DisplayName = "Given existing sales When listing Then returns paged list")]
    public async Task Handle_ReturnsPagedSales()
    {
        // Arrange
        var mapper = CreateMapper();
        var context = DbContextHelper.CreateInMemoryContext();

        var s1 = SaleHandlerTestData.CreateSaleEntity();
        var s2 = SaleHandlerTestData.CreateSaleEntity();
        s2.Id = Guid.NewGuid();
        s2.SaleNumber = 2;

        context.Sales.AddRange(s1, s2);
        await context.SaveChangesAsync();

        var handler = new ListSalesHandler(context, mapper);
        var query = SaleHandlerTestData.CreateListQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.TotalItems.Should().Be(2);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(1);
    }
}
