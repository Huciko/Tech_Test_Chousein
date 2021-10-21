using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.Email
{
    public interface IEmail
    {
        Task SendAsync(string to, string subject, string html);
    }
}
