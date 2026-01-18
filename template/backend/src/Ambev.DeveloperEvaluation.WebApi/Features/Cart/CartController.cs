using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCartRequest request, [FromServices] IMapper mapper)
    {
        var command = mapper.Map<CreateCartCommand>(request);

        var result = await _mediator.Send(command);

        var response = mapper.Map<CreateCartResponse>(result);

        return Created($"/api/carts/{response.Id}", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
       [FromQuery(Name = "_page")] int page = 1,
       [FromQuery(Name = "_size")] int size = 10,
       [FromQuery(Name = "_order")] string? order = null)
    {
        var request = new ListCartsRequest
        {
            Page = page,
            Size = size,
            Order = order
        };

        var query = _mapper.Map<ListCartsQuery>(request);

        var result = await _mediator.Send(query);

        var response = _mapper.Map<ListCartsResponse>(result);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, IMapper mapper)
    {
        var result = await _mediator.Send(new GetCartQuery { Id = id });
        var response = mapper.Map<GetCartResponse>(result);

        return Ok(response
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCartRequest request)
    {
        var command = _mapper.Map<UpdateCartCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);
        var response = _mapper.Map<UpdateCartResponse>(result);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, IMapper mapper)
    {
        var command = new DeleteCartCommand { Id = id };

        var result = await _mediator.Send(command);

        var response = mapper.Map<DeleteCartResponse>(result);

        return Ok(response);
    }


}
