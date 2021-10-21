using AcmeCaseAPI.Application.Endpoints.Case.Models;
using AcmeCaseAPI.Application.Endpoints.Case.Queries;
using AcmeCaseAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Controllers
{
    [ApiVersion("1.0")]
    public class CaseController : ApiController
    {
        // GET: /api/v1/Case
        /// <summary>
        /// Returns all cases 
        /// </summary>
        /// <remarks>Returns all cases</remarks>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("acmecase.scope")]
        [HttpGet(Name = "GetClientCases")]
        public async Task<ActionResult<IEnumerable<CaseDataModel>>> Get()
        {
            return Ok(await Mediator.Send(new GetClientCasesQuery()));
        }
    }
}
