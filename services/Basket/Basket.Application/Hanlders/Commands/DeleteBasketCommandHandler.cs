using AutoMapper;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;
namespace Basket.Application.Hanlders.Commands;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Unit>
{
    private readonly IBasketRepository _basketRepo;

    public DeleteBasketCommandHandler(IBasketRepository basketRepo)
    {
        _basketRepo = basketRepo;
    }
    public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await _basketRepo.DeleteBasketAsync(request.UserName);
        return Unit.Value;
    }
}
