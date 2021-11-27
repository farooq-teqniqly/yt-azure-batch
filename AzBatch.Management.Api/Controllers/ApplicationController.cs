// <copyright file="ApplicationController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teqniqly.AzBatch.Management.Abstractions;
    using Teqniqly.AzBatch.Management.Infrastructure;

    [ApiController]
    [Route("application")]
    public class ApplicationController : ControllerBase
    {
        private readonly IBatchManagementService batchManagementService;
        private readonly ILogger<ApplicationController> logger;

        public ApplicationController(
            IBatchManagementService batchManagementService,
            ILogger<ApplicationController> logger)
        {
            this.batchManagementService = batchManagementService;
            this.logger = logger;
        }

        [HttpDelete]
        [Route("{applicationName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(string))]
        public async Task<IActionResult> DeleteApplicationAsync(string applicationName)
        {
            try
            {
                await this.batchManagementService.DeleteApplicationAsync(applicationName);
                return this.NoContent();
            }
            catch (BatchManagementServiceException batchManagementServiceException)
            {
                this.logger.LogError(
                    batchManagementServiceException,
                    batchManagementServiceException.Message);

                return this.BadRequest(batchManagementServiceException.Message);
            }
        }
    }
}
