using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public sealed class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<Sale, CancelSaleResult>();
    }
}
