using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public sealed class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleRequest.SaleItemRequest, UpdateSaleCommand.SaleItemCommand>();

        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<UpdateSaleResult.SaleItemResult, UpdateSaleResponse.SaleItemResponse>();
    }
}
