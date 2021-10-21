using System.Collections.Generic;

namespace AcmeCaseAPI.Application.Common.ContextUser
{
    public interface IContextUser
    {
        string GetClientID();
        int GetContextUserID();
        string GetContextUserName();
        IEnumerable<int> GetContextUserRoles();
        string GetContextUserIP();
    }
}
