using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepo, 
        IMapper mapper, 
        ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);
        var genertedOrder = await _orderRepo.CreateAsync(order);
        _logger.LogInformation($"Order with id {genertedOrder.Id} is added successflly");
        return genertedOrder.Id;
    }
}
