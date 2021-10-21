using AcmeCaseAPI.Application.Common.ContextUser;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.Behaviour
{
    public class RequestLoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IContextUser _user;
        private readonly IConfiguration _config;

        public RequestLoggerBehaviour(ILogger<TRequest> logger, IContextUser user, IConfiguration config)
        {
            _logger = logger;
            _user = user;
            _config = config;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.GetContextUserID();
            string userName = _user.GetContextUserName();
            bool logEndpointCalls = _config.GetValue<bool>("LogEndpointCalls");

            if (logEndpointCalls)
            {
                _logger.LogInformation("API Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
            }

            await Task.FromResult(request);

        }
    }
}
