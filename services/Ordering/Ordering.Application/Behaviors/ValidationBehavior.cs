

using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            // get validation rules one by one
            var validationRules = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

            // check for failures
            var failures = validationRules.SelectMany(v => v.Errors).Where(f => f != null).ToList();
            
            // if there is failures so will throw the custom validation exception class which we created
            if (failures.Count != 0)
                throw new ValidationException(failures);
        }
            return await next();
    }
}
