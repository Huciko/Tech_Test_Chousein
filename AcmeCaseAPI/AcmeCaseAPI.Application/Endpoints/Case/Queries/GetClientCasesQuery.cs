using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Endpoints.Case.Models;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;


namespace AcmeCaseAPI.Application.Endpoints.Case.Queries
{

    public class GetClientCasesQuery : IRequest<IEnumerable<CaseDataModel>>
    {

        public class GetClientCasesQueryHandler : IRequestHandler<GetClientCasesQuery, IEnumerable<CaseDataModel>>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public GetClientCasesQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<IEnumerable<CaseDataModel>> Handle(GetClientCasesQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters();
                paramList.Add("@ClientIdentifier", _user.GetClientID());

                IEnumerable<CaseDataModel> cases;

                using (IDbConnection connection = _conn.Create())
                {
                    cases = await connection.QueryAsync<CaseDataModel>("Get_Client_Cases", paramList, commandType: CommandType.StoredProcedure);
                }

                return cases;

            }
        }
    }

}
