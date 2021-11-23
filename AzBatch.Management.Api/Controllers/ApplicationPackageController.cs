// <copyright file="ApplicationPackageController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Teqniqly.AzBatch.Management.Abstractions;

    [ApiController]
    [Route("package")]
    public class ApplicationPackageController : ControllerBase
    {
        private readonly IBatchManagementService batchManagementService;

        public ApplicationPackageController(IBatchManagementService batchManagementService)
        {
            this.batchManagementService = batchManagementService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CreateApplicationPackageAsync(
            [FromBody] CreateApplicationPackageRequest request)
        {
            var result = await this.batchManagementService.CreateApplicationPackageAsync(
                request.ApplicationPackageName,
                request.ApplicationPackageVersion);

            return this.Accepted(new CreateApplicationPackageResponse
            {
                StorageUrl = result.Url,
                StorageUrlExpiresOn = result.ExpiresOn,
                ApplicationPackageState = result.State,
            });
        }

        [HttpDelete]
        [Route("{applicationPackageName}/{applicationPackageVersion}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteApplicationPackageAsync(
            string applicationPackageName,
            string applicationPackageVersion)
        {
            await this.batchManagementService.DeleteApplicationPackageAsync(
                applicationPackageName,
                applicationPackageVersion);

            return this.NoContent();
        }
    }
}
