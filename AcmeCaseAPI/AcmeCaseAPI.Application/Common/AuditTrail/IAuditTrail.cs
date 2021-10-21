using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.AuditTrail
{
    public interface IAuditTrail<in TRequest, in TResponse>
    {
        Task LogAsync(TRequest request, TResponse response);
    }
}
