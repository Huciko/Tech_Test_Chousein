using AcmeCaseAPI.Application.Endpoints.Case.Queries;
using AcmeCaseAPI.Application.Endpoints.File.Models;
using AcmeCaseAPI.Application.Endpoints.File.Queries;
using AcmeCaseAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Controllers.v1
{
    /// <summary>
    /// File Controller
    /// </summary>
    [ApiVersion("1.0")]
    public class FileController : ApiController
    {
        // GET: /api/v1/File
        /// <summary>
        /// Returns all files info
        /// </summary>
        /// <remarks>Returns all files info</remarks>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("acmecase.scope")]
        [HttpGet(Name = "GetAllFiles")]
        public async Task<ActionResult<IEnumerable<FileInfoDataModel>>> Get()
        {
            return Ok(await Mediator.Send(new GetAllFilesInfoQuery()));
        }

        // GET: /api/v1/File/Case/1
        /// <summary>
        /// Returns all files info for a case 
        /// </summary>
        /// <remarks>Returns all files info for a case</remarks>
        /// <param name="caseID">Case id</param>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("acmecase.scope")]
        [HttpGet("case/{caseID}", Name = "GetCaseFiles")]
        public async Task<ActionResult<IEnumerable<FileInfoDataModel>>> Get(int caseID)
        {
            bool caseIsValid = await Mediator.Send(new ValidateCaseToClientQuery { CaseID = caseID });

            if (caseIsValid == false)
            {
                // create link to suggest for valid cases
                var casesEnpoint = new Uri(Url.Link("GetClientCases", null)).AbsoluteUri;
                return BadRequest($"caseID is not valid. For a list of valid cases query {casesEnpoint}");
            }

            return Ok(await Mediator.Send(new GetCaseFilesInfoQuery { CaseID = caseID }));
        }

        // GET: /api/v1/File/1/Download
        /// <summary>
        /// Returns <see cref="Stream"/> 
        /// </summary>
        /// <remarks>Returns file</remarks>
        /// <param name="fileID">File id</param>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("acmecase.scope")]
        [HttpGet("{fileID}/download", Name = "DownloadFile")]
        public async Task<ActionResult<Stream>> Download(int fileID)
        {

            bool fileIsValid = await Mediator.Send(new ValidateFileToClientQuery { FileID = fileID });

            if (fileIsValid == false)
            {
                // create link to suggest for valid files
                var allFilesEndpoint = new Uri(Url.Link("GetAllFiles", null)).AbsoluteUri;
                return BadRequest($"fileID is not valid. For a list of valid files query {allFilesEndpoint}");
            }

            var file = await Mediator.Send(new GetFileForDownloadQuery { FileID = fileID });
            MemoryStream ms = new MemoryStream(file.FileData);
            return new FileStreamResult(ms, $"application/{file.FileTypeExtension}");
        }
    }
}
