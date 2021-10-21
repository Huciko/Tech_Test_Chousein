using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Endpoints.File.Models;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Endpoints.File.Queries
{
    public class GetCaseFilesInfoQuery : IRequest<IEnumerable<FileInfoDataModel>>
    {
        public int CaseID { get; set; }

        public class GetCaseFilesInfoQueryHandler : IRequestHandler<GetCaseFilesInfoQuery, IEnumerable<FileInfoDataModel>>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public GetCaseFilesInfoQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<IEnumerable<FileInfoDataModel>> Handle(GetCaseFilesInfoQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters(query);

                IEnumerable<FileInfoDataModel> fileInfo;

                using (IDbConnection connection = _conn.Create())
                {
                    fileInfo = await connection.QueryAsync<FileInfoDataModel>("Get_Case_Files_Info", paramList, commandType: CommandType.StoredProcedure);
                }

                return fileInfo;

            }
        }
    }
}
