using System;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Patterns.Mediator.Behaviours
{
    public class ExceptionHandler<TRequest, TResponse, TException> : RequestExceptionHandler<TRequest, TResponse, TException> where TException : Exception
    {
        private readonly ILogger<ExceptionHandler<TRequest, TResponse, TException>> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler<TRequest, TResponse, TException>> logger)
        {
            _logger = logger;
        }

        protected override void Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state)
        {
            var response = Activator.CreateInstance<TResponse>();

            _logger.LogError(
                "Error: RequestId:{@RequestId} Request:{{{@Request}}} Time:{@Time} Message:{@Message} Errors:{@Errors}"
            );
            
            state.SetHandled(response);
        }
    }
}