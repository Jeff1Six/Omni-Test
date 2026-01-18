using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SalesFeature.SaleCreate;

public sealed class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleRequest.CreateSaleItemRequest, CreateSaleCommand.CreateSaleItemCommand>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleResult.SaleItemResult, CreateSaleResponse.SaleItemResponse>();
    }
}
