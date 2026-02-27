
using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class ValidationException : ApplicationException
{
    public Dictionary<string, string[]> Errors { get; }
    public ValidationException() : base("one Error or more Occured")
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException(List<ValidationFailure> failures):this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage).ToDictionary(
            failure => failure.Key, failure => failure.ToArray());
    }
}
