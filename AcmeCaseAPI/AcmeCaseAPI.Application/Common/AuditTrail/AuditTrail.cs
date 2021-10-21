using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Common.Email;
using AcmeCaseAPI.Persistence;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;
namespace AcmeCaseAPI.Application.Common.AuditTrail
{
    public class AuditTrail<TRequest, TResponse> : IAuditTrail<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly IDbConnect _conn;
        private readonly IContextUser _user;
        private readonly IConfiguration _config;
        private readonly IEmail _email;

        public AuditTrail(ILogger<TRequest> logger, IDbConnect conn, IContextUser user, IConfiguration config, IEmail email)
        {
            _logger = logger;
            _conn = conn;
            _user = user;
            _config = config;
            _email = email;
        }

        public async Task LogAsync(TRequest request, TResponse response)
        {
            bool logRequest, logResponse;
            string logRequestTo;

            int userID = _user.GetContextUserID();
            string userName = _user.GetContextUserName();
            string requestName = typeof(TRequest).Name;

            try
            {
                bool requestIsQuery = requestName.EndsWith("Query", StringComparison.OrdinalIgnoreCase); // it is important to follow the current naming convention for the application requests
                bool requestIsCommand = requestName.EndsWith("Command", StringComparison.OrdinalIgnoreCase); // it is important to follow the current naming convention for the application commands

                if (requestIsQuery) // query
                {
                    logRequestTo = _config.GetValue<string>("AuditTrailSettings:QuerySettings:LogQueryTo");
                    logRequest = _config.GetValue<bool>("AuditTrailSettings:QuerySettings:LogQueryRequest");
                    logResponse = _config.GetValue<bool>("AuditTrailSettings:QuerySettings:LogQueryResponse");
                }
                else // command
                {
                    logRequestTo = _config.GetValue<string>("AuditTrailSettings:CommandSettings:LogCommandTo");
                    logRequest = _config.GetValue<bool>("AuditTrailSettings:CommandSettings:LogCommandRequest");
                    logResponse = _config.GetValue<bool>("AuditTrailSettings:CommandSettings:LogCommandResponse");
                }

                if (logRequest || logResponse)
                {

                    if (logRequestTo == "File") // File
                    {
                        if (logRequest)
                        {
                            _logger.LogInformation("Acme Case API Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userID, userName, request);
                        }
                        if (logResponse)
                        {
                            _logger.LogInformation("Acme Case API Request: {Name} {@UserId} {@UserName} {@Response}", requestName, userID, userName, response);
                        }
                    }
                    else // DB
                    {
                        DynamicParameters paramList = new DynamicParameters();
                        paramList.Add("@UserID", userID);
                        paramList.Add("@UserName", userName);
                        paramList.Add("@IPAddress", _user.GetContextUserIP());
                        paramList.Add("@RequestName", requestName);

                        if (logRequest)
                        {
                            paramList.Add("@Request", JsonConvert.SerializeObject(request));
                        }
                        if (logResponse)
                        {
                            paramList.Add("@Response", JsonConvert.SerializeObject(response));
                        }

                        using (IDbConnection connection = _conn.Create())
                        {
                            await connection.ExecuteAsync("Add_Audit_Trail", paramList, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // send email if DB Audit fails
                string mailTo = _config.GetValue<string>("ExceptionLoggingSettings:To");
                string subject = "Acme Case API Audit Trail Failed";
                string mailBody = $"Acme Case API Audit Trail Failed For Request: {requestName} {userID} {userName} <br><br>Request: {JsonConvert.SerializeObject(request)} <br><br>Response: {JsonConvert.SerializeObject(response)} <br><br>Exception: {ex}";

                await _email.SendAsync(mailTo, subject, mailBody);

                throw;
            }

        }
    }
}
