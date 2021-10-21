using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Common.Email;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;


namespace AcmeCaseAPI.Application.Common.Behaviour
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IDbConnect _conn;
        private readonly IContextUser _user;
        private readonly IConfiguration _config;
        private readonly IEmail _email;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IDbConnect conn, IContextUser user, IConfiguration config, IEmail email)
        {
            _logger = logger;
            _conn = conn;
            _user = user;
            _config = config;
            _email = email;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {

                string requestName = typeof(TRequest).Name;
                var userID = _user.GetContextUserID();
                string userName = _user.GetContextUserName();

                // check settings in the appsettings.json file on how to log
                string logExceptionTo = _config.GetValue<string>("ExceptionLoggingSettings:LogExceptionTo");
                bool sendEmail = _config.GetValue<bool>("ExceptionLoggingSettings:SendEmail");
                string mailTo = _config.GetValue<string>("ExceptionLoggingSettings:To");
                string subject, mailBody;

                if (sendEmail)
                {
                    subject = "Acme Case API Exception";
                    mailBody = $"Acme Case API Exception For Request: {requestName} {userID} {userName} <br>Request: {JsonConvert.SerializeObject(request)} <br><br> Exception: {ex}";
                    await _email.SendAsync(mailTo, subject, mailBody);
                }

                if (logExceptionTo == "File") // File
                {
                    _logger.LogError(ex, "Acme Case API Request: Unhandled Exception for Request {Name} {@UserId} {@UserName} {@Request}", requestName, userID, userName, request);
                }
                else // DB
                {
                    DynamicParameters paramList = new DynamicParameters();
                    paramList.Add("@UserID", userID);
                    paramList.Add("@UserName", userName);
                    paramList.Add("@IPAddress", _user.GetContextUserIP());
                    paramList.Add("@RequestName", requestName);
                    paramList.Add("@Request", JsonConvert.SerializeObject(request));
                    paramList.Add("@Exception", ex.ToString());

                    using (IDbConnection connection = _conn.Create())
                    {
                        try
                        {
                            await connection.ExecuteAsync("Add_Error_Log", paramList, commandType: CommandType.StoredProcedure);
                        }
                        catch (Exception errorLogException)
                        {
                            // log reason to file if DB fails
                            _logger.LogError(errorLogException, "Acme Case API error logging exception: Unhandled Exception for Request {Name} {@UserId} {@UserName} {@Request}", requestName, userID, userName, request);

                            // and send email
                            subject = "Acme Case API Error Log to DB Failed";
                            mailBody = $"Acme Case API Error Log to DB Failed For Request: {requestName} {userID} {userName} {JsonConvert.SerializeObject(request)} <br><br> Exception: {errorLogException}";
                            await _email.SendAsync(mailTo, subject, mailBody);

                            // throw;
                        }
                    }

                }

                throw;

                //// to avoid (throw) nested exception handling
                //TResponse res = default(TResponse);
                //return await Task.FromResult(res);

            }

        }
    }
}
