
using MediatR;

namespace Basket.Application.Commands;

public class DeleteBasketCommand(string userName) : IRequest<Unit>
{
    public string UserName { get; set; } = userName;
}
