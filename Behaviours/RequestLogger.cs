using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Patterns.Mediator.Behaviours
{
    public class RequestLogger<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestLogger<TRequest, TResponse>> _logger;

        public RequestLogger(ILogger<RequestLogger<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(
                "Handle: RequestId:{@RequestId} Request:{{{@Request}}} Time:{@Time} ResquestedBy:{@UserId} RequestedFor:{@ApplicationId}"
            );

            var response = await next();

            _logger.LogInformation(
                "Handled: RequestId:{@RequestId} Request:{{{@Request}}} Time:{@Time} ResquestedBy:{@UserId} RequestedFor:{@ApplicationId}"
            );
            
            return response;
        }
    }
}