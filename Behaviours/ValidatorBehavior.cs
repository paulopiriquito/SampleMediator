using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Patterns.Mediator.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Patterns.Mediator.Behaviours
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;

        public ValidatorBehavior(
            IValidator<TRequest>[] validators,
            ILogger<ValidatorBehavior<TRequest, TResponse>> logger
        )
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var fullTypeName = request.GetTypeFullName();

            _logger.LogInformation("Validating action {ActionType}", fullTypeName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (!failures.Any()) return await next();
            
            _logger.LogWarning(
                "Validation errors - {ActionType} - Action: {Action} - Errors: {ValidationErrors}",
                fullTypeName,
                request,
                failures
            );

            throw new ValidationException($"Validation Failed for request {fullTypeName}", failures);
        }
    }
}