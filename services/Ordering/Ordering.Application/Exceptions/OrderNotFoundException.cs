
namespace rdering.Application.Exceptions;

public class OrderNotFoundException : ApplicationException
{
    public OrderNotFoundException(string name, Object key) :base($"Entity with name {name} and id {key} is Not found")
    {
        
    }
}
