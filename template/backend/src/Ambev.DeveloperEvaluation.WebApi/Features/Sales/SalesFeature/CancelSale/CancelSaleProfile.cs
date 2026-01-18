using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public sealed class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}
