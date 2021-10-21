using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Endpoints.Case.Queries
{
    /// <summary>
    /// Checks whether the client has access to the specified case.
    /// </summary>
    public class ValidateCaseToClientQuery : IRequest<bool>
    {
        public int CaseID { get; set; }

        public class ValidateCaseToClientQueryHandler : IRequestHandler<ValidateCaseToClientQuery, bool>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public ValidateCaseToClientQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<bool> Handle(ValidateCaseToClientQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters(query);
                paramList.Add("@ClientIdentifier", _user.GetClientID());

                bool isValid;

                using (IDbConnection connection = _conn.Create())
                {
                    isValid = await connection.ExecuteScalarAsync<bool>("Validate_Case_To_Client", paramList, commandType: CommandType.StoredProcedure);
                }

                return isValid;

            }
        }
    }
}
