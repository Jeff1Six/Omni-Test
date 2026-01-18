using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public sealed class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<SaleItem, UpdateSaleResult.SaleItemResult>();

        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}
