using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Common.Email;
using AcmeCaseAPI.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.Behaviour
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger _logger;
        private readonly IDbConnect _conn;
        private readonly IContextUser _user;
        private readonly IConfiguration _config;
        private readonly IEmail _email;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger, IDbConnect conn, IContextUser user, IConfiguration config, IEmail email)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _conn = conn;
            _user = user;
            _config = config;
            _email = email;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            int timeLimit = _config.GetValue<int>("RequestPerformanceBehaviourSettings:TimeLimit");

            if (elapsedMilliseconds > timeLimit) // timeLimit is set in the appsettings.json file in AcmeCaseAPI project
            {
                var requestName = typeof(TRequest).Name;
                var userId = _user.GetContextUserID();
                string userName = _user.GetContextUserName();

                // mail or log
                string warnType = _config.GetValue<string>("RequestPerformanceBehaviourSettings:WarnType");
                if (warnType == "Email")
                {
                    string mailTo = _config.GetValue<string>("RequestPerformanceBehaviourSettings:To");
                    string subject = _config.GetValue<string>("RequestPerformanceBehaviourSettings:Subject");
                    string mailBody = $"Time limit set: {timeLimit} <br><br>API Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {userId} {userName} <br><br> Request: {JsonConvert.SerializeObject(request)} <br><br> Response: {JsonConvert.SerializeObject(response)}";

                    await _email.SendAsync(mailTo, subject, mailBody);
                }
                else
                {
                    _logger.LogWarning("API Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);
                }
            }

            return response;
        }
    }
}
