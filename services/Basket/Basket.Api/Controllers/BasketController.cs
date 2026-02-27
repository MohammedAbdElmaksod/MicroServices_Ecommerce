using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[ApiVersion("1")]
public class BasketController : BaseController
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

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketQuery(userName);
        var basket = await _mediator.Send(query);
        return Ok(basket);
    }
  
    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket(CreateShoppingCartCommand command)
    {
        var basket = await _mediator.Send(command);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasket")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        var command = new DeleteBasketCommand(userName);
        return Ok(await _mediator.Send(command));
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
    {
        var query = new GetBasketQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);

        if (basket == null)
            return BadRequest();

        var eventMsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMsg.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMsg);

        var deletedCmd = new DeleteBasketCommand(basketCheckout.UserName);
        await _mediator.Send(deletedCmd);
        return Accepted();
    }
}
