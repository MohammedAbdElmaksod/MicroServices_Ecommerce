using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers.V2;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IMediator mediator, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }


    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout(BasketCheckoutV2 basketCheckout)
    {
        var query = new GetBasketQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);

        if (basket == null)
            return BadRequest();

        var eventMsg = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
        eventMsg.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMsg);

        var deletedCmd = new DeleteBasketCommand(basketCheckout.UserName);
        await _mediator.Send(deletedCmd);
        return Accepted();
    }
}
