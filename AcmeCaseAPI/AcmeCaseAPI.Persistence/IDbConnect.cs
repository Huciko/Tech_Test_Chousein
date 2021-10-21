using System.Data.Common;

namespace AcmeCaseAPI.Persistence
{
    public interface IDbConnect
    {
        DbConnection Create();
    }
}
