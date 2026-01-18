using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

public static class SaleHandlerTestData
{
    public static Sale CreateSaleEntity()
    {
        return new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = 1,
            Date = new DateTime(2026, 01, 16, 0, 0, 0, DateTimeKind.Utc),
            CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            CustomerName = "Customer A",
            BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            BranchName = "Branch B",
            Cancelled = false,
            TotalAmount = 0m,
            Items = new List<SaleItem>()
        };
    }

    public static SaleItem CreateActiveItem(Guid saleId, Guid productId, int quantity, decimal unitPrice)
    {
        var discountPercent = SalesRulesService.GetDiscountPercentage(quantity);
        var discountAmount = unitPrice * quantity * discountPercent;
        var totalItemAmount = (unitPrice * quantity) - discountAmount;

        return new SaleItem
        {
            Id = Guid.NewGuid(),
            SaleId = saleId,
            ProductId = productId,
            ProductTitle = $"Product {productId}",
            Quantity = quantity,
            UnitPrice = unitPrice,
            DiscountPercent = discountPercent,
            DiscountAmount = discountAmount,
            TotalItemAmount = totalItemAmount,
            Cancelled = false
        };
    }

    public static decimal CalcTotalItemAmount(int qty, decimal unitPrice, decimal discountPercent)
    {
        var discountAmount = unitPrice * qty * discountPercent;
        return (unitPrice * qty) - discountAmount;
    }

    public static CreateSaleCommand CreateValidCreateCommand()
    {
        return new CreateSaleCommand
        {
            Date = new DateTime(2026, 01, 16, 0, 0, 0, DateTimeKind.Utc),
            CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Items = new List<CreateSaleCommand.CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Quantity = 4,
                    UnitPrice = 10m
                }
            }
        };
    }

    public static UpdateSaleCommand CreateUpdateCommand(Guid saleId)
    {
        return new UpdateSaleCommand
        {
            Id = saleId,
            Date = new DateTime(2026, 01, 16, 0, 0, 0, DateTimeKind.Utc),
            CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Items = new List<UpdateSaleCommand.SaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Quantity = 4,
                    UnitPrice = 10m
                }
            }
        };
    }

    public static UpdateSaleCommand CreateUpdateCommand_WithQty12_20Percent(Guid saleId)
    {
        return new UpdateSaleCommand
        {
            Id = saleId,
            Date = new DateTime(2026, 01, 16, 0, 0, 0, DateTimeKind.Utc),
            CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Items = new List<UpdateSaleCommand.SaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Quantity = 12,
                    UnitPrice = 120m
                }
            }
        };
    }

    public static GetSaleQuery CreateGetByIdQuery(Guid saleId)
        => new() { Id = saleId };

    public static ListSalesQuery CreateListQuery()
        => new() { Page = 1, Size = 10, Order = "id asc" };

    public static CancelSaleCommand CreateDeleteCommand(Guid saleId)
        => new() { Id  = saleId };
}
