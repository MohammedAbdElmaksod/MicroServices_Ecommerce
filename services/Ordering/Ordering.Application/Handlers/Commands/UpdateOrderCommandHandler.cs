
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using rdering.Application.Exceptions;

namespace Ordering.Application.Handlers.Commands;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepo;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IOrderRepository orderRepo, ILogger<UpdateOrderCommandHandler> logger, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetByIdAsync(request.Id);
        if (order == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
        _mapper.Map(request, order);
        await _orderRepo.UpdateAsync(order);
        _logger.LogInformation($"Order with id {request.Id} is Updated Successuflly");
        return Unit.Value;
    }
}
