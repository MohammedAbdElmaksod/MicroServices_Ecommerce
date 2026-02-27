
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using rdering.Application.Exceptions;

namespace Ordering.Application.Handlers.Commands;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepo;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepo, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepo = orderRepo;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetByIdAsync(request.Id);
        if (order == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
        await _orderRepo.DeleteAsync(order);
        _logger.LogInformation($"Order with id {request.Id} is Deleted Successuflly");
        return Unit.Value;
    }
}
