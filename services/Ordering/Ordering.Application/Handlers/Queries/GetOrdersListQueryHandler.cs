using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Queries;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;
    public GetOrdersListQueryHandler(IOrderRepository orderRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<List<OrderResponse>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var ordersList = await _orderRepo.GetOrderByUserName(request.UserName);
        return _mapper.Map<List<OrderResponse>>(ordersList);
    }
}
