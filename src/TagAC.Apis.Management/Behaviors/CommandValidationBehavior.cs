using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Management.Domain.Commands;

namespace TagAC.Apis.Management.Behaviors
{
    public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : CommandResponse, new()
    {
        private readonly ILogger<CommandValidationBehavior<TRequest, TResponse>> logger;
        private readonly IValidationHandler<TRequest> validationHandler;

        public CommandValidationBehavior(ILogger<CommandValidationBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public CommandValidationBehavior(ILogger<CommandValidationBehavior<TRequest, TResponse>> logger, IValidationHandler<TRequest> validationHandler)
        {
            this.logger = logger;
            this.validationHandler = validationHandler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType();
            if (validationHandler == null)
            {
                logger.LogInformation("{Request} does not have a validation handler configured.", requestName);
                return await next();
            }

            var result = await validationHandler.Validate(request);
            if (!result.IsSuccessful)
            {
                logger.LogWarning("Validation failed for {Request}. Error: {Error}", requestName, result.Error);
                return new TResponse { StatusCode = HttpStatusCode.BadRequest, ErrorMessage = result.Error };
            }

            logger.LogInformation("Validation successful for {Request}.", requestName);
            return await next();
        }
    }
}
