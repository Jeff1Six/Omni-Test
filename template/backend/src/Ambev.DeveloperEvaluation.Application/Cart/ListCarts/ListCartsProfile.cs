using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<CartItem, ListCartsResult.CartSummaryResult.CartProductResult>();

        CreateMap<Cart, ListCartsResult.CartSummaryResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
