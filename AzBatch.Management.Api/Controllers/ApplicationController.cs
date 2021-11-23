// <copyright file="ApplicationController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Teqniqly.AzBatch.Management.Abstractions;

    [ApiController]
    [Route("application")]
    public class ApplicationController : ControllerBase
    {
        private readonly IBatchManagementService batchManagementService;

        public ApplicationController(IBatchManagementService batchManagementService)
        {
            this.batchManagementService = batchManagementService;
        }

        [HttpDelete]
        [Route("{applicationName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteApplicationAsync(string applicationName)
        {
            await this.batchManagementService.DeleteApplicationAsync(applicationName);
            return this.NoContent();
        }
    }
}
