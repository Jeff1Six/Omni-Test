using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

public sealed class CancelSaleItemProfile : Profile
{
    public CancelSaleItemProfile()
    {
        CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}
