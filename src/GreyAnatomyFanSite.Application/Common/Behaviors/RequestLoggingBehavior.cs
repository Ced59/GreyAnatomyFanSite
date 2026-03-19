using MediatR;
using Microsoft.Extensions.Logging;

namespace GreyAnatomyFanSite.Application.Common.Behaviors
{
    public sealed class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger;

        public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;

            logger.LogInformation("MediatR request started: {RequestName}", requestName);

            TResponse response = await next();

            logger.LogInformation("MediatR request completed: {RequestName}", requestName);

            return response;
        }
    }
}
