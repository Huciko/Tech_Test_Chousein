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
    public class GetAllFilesInfoQuery : IRequest<IEnumerable<FileInfoDataModel>>
    {
        public class GetAllFilesInfoQueryHandler : IRequestHandler<GetAllFilesInfoQuery, IEnumerable<FileInfoDataModel>>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public GetAllFilesInfoQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<IEnumerable<FileInfoDataModel>> Handle(GetAllFilesInfoQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters();
                paramList.Add("@ClientIdentifier", _user.GetClientID());

                IEnumerable<FileInfoDataModel> fileInfo;

                using (IDbConnection connection = _conn.Create())
                {
                    fileInfo = await connection.QueryAsync<FileInfoDataModel>("Get_All_Files_Info", paramList, commandType: CommandType.StoredProcedure);
                }

                return fileInfo;

            }
        }
    }
}
