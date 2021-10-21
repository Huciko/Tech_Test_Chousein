using AcmeCaseAPI.Application.Common.AuditTrail;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.Behaviour
{
    public class RequestPostProcessorBehaviour<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly IAuditTrail<TRequest, TResponse> _auditTrail;

        public RequestPostProcessorBehaviour(IAuditTrail<TRequest, TResponse> auditTrail)
        {
            _auditTrail = auditTrail;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            await _auditTrail.LogAsync(request, response);
        }

    }
}
