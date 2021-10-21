using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Endpoints.File.Models;
using AcmeCaseAPI.Persistence;
using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Endpoints.File.Queries
{
    public class GetFileForDownloadQuery : IRequest<FileForDownloadDataModel>
    {
        public int FileID { get; set; }

        public class GetFileForDownloadQueryHandler : IRequestHandler<GetFileForDownloadQuery, FileForDownloadDataModel>
        {
            private readonly IDbConnect _conn;
            private readonly IContextUser _user;

            public GetFileForDownloadQueryHandler(IDbConnect conn, IContextUser user)
            {
                _conn = conn;
                _user = user;
            }

            public async Task<FileForDownloadDataModel> Handle(GetFileForDownloadQuery query, CancellationToken cancellationToken)
            {

                DynamicParameters paramList = new DynamicParameters(query);

                FileForDownloadDataModel file;

                using (IDbConnection connection = _conn.Create())
                {
                    file = await connection.QuerySingleOrDefaultAsync<FileForDownloadDataModel>("Get_File_For_Download", paramList, commandType: CommandType.StoredProcedure);
                }

                return file;

            }
        }
    }
}
