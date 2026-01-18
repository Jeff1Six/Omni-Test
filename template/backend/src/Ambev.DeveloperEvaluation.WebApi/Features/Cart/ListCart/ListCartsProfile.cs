using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<ListCartsRequest, ListCartsQuery>();

        CreateMap<ListCartsResult, ListCartsResponse>();
        CreateMap<ListCartsResult.CartSummaryResult, ListCartsResponse.CartResponseItem>();
        CreateMap<ListCartsResult.CartSummaryResult.CartProductResult,
                  ListCartsResponse.CartResponseItem.CartProductResponse>();
    }
}
