using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.SalesFeature.SaleCreate;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public sealed class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;

    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSalesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] ListSalesRequest request, IMapper mapper)
    {
        var query = mapper.Map<ListSalesQuery>(request);

        var result = await _mediator.Send(query);

        var response = mapper.Map<ListSalesResponse>(result);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetSaleQuery { Id = id };

        var result = await _mediator.Send(query);
        var response = _mapper.Map<GetSaleResponse>(result);

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, [FromServices] IMapper mapper)
    {
        var command = mapper.Map<CreateSaleCommand>(request);

        var result = await _mediator.Send(command);

        var response = mapper.Map<CreateSaleResponse>(result);

        return Created($"/sales/{response.Id}", response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request, IMapper mapper)
    {
        var command = mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        var response = mapper.Map<UpdateSaleResponse>(result);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, IMapper mapper)
    {
        var command = new CancelSaleCommand { Id = id };

        var result = await _mediator.Send(command);
        var response = mapper.Map<CancelSaleResponse>(result);

        return Ok(response);
    }

    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> CancelItem(Guid id, Guid itemId, IMapper mapper)
    {
        var command = new CancelSaleItemCommand
        {
            SaleId = id,
            ItemId = itemId
        };

        var result = await _mediator.Send(command);
        var response = mapper.Map<CancelSaleItemResponse>(result);

        return Ok(response);
    }
}
