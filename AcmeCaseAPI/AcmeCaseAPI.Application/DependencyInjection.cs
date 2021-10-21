using AcmeCaseAPI.Application.Common.AuditTrail;
using AcmeCaseAPI.Application.Common.Behaviour;
using AcmeCaseAPI.Application.Common.ContextUser;
using AcmeCaseAPI.Application.Common.Email;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AcmeCaseAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayerDependencies(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddScoped(typeof(IAuditTrail<,>), typeof(AuditTrail<,>));
            services.AddScoped<IEmail, Email>();
            services.AddScoped<IContextUser, ContextUser>();

            return services;
        }
    }
}
