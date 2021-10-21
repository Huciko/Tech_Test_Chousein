using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;


namespace AcmeCaseAPI.Application.Endpoints.File.Queries
{
    /// <summary>
    /// Checks whether the client has access to the specified file.
    /// </summary>
    public class ValidateFileToClientQuery : IRequest<bool>
    {
        public int FileID { get; set; }

        public class ValidateFileToClientQueryHandler : IRequestHandler<ValidateFileToClientQuery, bool>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public ValidateFileToClientQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<bool> Handle(ValidateFileToClientQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters(query);
                paramList.Add("@ClientIdentifier", _user.GetClientID());

                bool isValid;

                using (IDbConnection connection = _conn.Create())
                {
                    isValid = await connection.ExecuteScalarAsync<bool>("Validate_File_To_Client", paramList, commandType: CommandType.StoredProcedure);
                }

                return isValid;

            }
        }
    }
}
