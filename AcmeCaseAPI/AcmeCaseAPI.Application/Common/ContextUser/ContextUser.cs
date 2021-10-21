using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AcmeCaseAPI.Application.Common.ContextUser
{
    public class ContextUser : IContextUser
    {
        private readonly IHttpContextAccessor _context;

        public ContextUser(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetClientID()
        {
            return _context.HttpContext.User?.Claims?.Where(c => c.Type == JwtClaimTypes.ClientId)
                .Select(c => c.Value).SingleOrDefault();
        }

        public int GetContextUserID()
        {
            if (_context.HttpContext == null) { return -1; }
            var contextUserID = _context.HttpContext.User?.Claims?.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault();
            return Convert.ToInt32(contextUserID);
        }

        public string GetContextUserName()
        {
            if (_context.HttpContext == null) { return "service"; }
            var contextUserName = _context.HttpContext.User?.Claims?.Where(c => c.Type == JwtClaimTypes.Name)
                 .Select(c => c.Value).SingleOrDefault();
            // incase the context is not a real user but an application then check for the JwtClaimTypes.ClientId
            if (contextUserName == null)
            {
                contextUserName = _context.HttpContext.User?.Claims?.Where(c => c.Type == JwtClaimTypes.ClientId)
               .Select(c => c.Value).SingleOrDefault();
            }
            return contextUserName;
        }

        public IEnumerable<int> GetContextUserRoles()
        {
            if (_context.HttpContext == null) { return Enumerable.Empty<int>(); }
            var contextUserRoles = _context.HttpContext.User?.Claims?.Where(c => c.Type == JwtClaimTypes.Role)
                 .Select(c => Convert.ToInt32(c.Value));
            return contextUserRoles;
        }

        public string GetContextUserIP()
        {
            if (_context.HttpContext == null) { return "service"; }
            return _context.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}
