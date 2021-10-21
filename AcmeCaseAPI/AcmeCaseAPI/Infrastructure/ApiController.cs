using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeCaseAPI.Infrastructure
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private ISender _mediator;

        public ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

    }
}
