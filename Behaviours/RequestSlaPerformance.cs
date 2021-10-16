using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Patterns.Mediator.Behaviours
{
    public class RequestSlaPerformance<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<RequestSlaPerformance<TRequest, TResponse>> _logger;
        private readonly int _longRunningRequestTime;

        public RequestSlaPerformance(ILogger<RequestSlaPerformance<TRequest, TResponse>> logger, IConfiguration configuration)
        {
            _timer = new Stopwatch();

            _logger = logger;

            var timeConfig = configuration.GetValue<int>("LogRequestSlaMilliseconds");

            _longRunningRequestTime = timeConfig != 0 ? timeConfig : 500;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            _logger.LogInformation(
                "Handling Performance: RequestId:{@RequestId} Request:{{{@Request}}} Time:{@Time} ExecutionTime:{@ExecutionTime} milliseconds"
            );

            if (_timer.ElapsedMilliseconds <= _longRunningRequestTime)
                return response;

            _logger.LogWarning(
                "Long Running Request: RequestId:{@RequestId} {ElapsedMilliseconds} milliseconds Request:{@Request}");

            return response;
        }
    }
}