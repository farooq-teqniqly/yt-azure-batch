// <copyright file="BatchJobController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teqniqly.AzBatch.Abstractions;
    using Teqniqly.AzBatch.Infrastructure;

    [ApiController]
    [Route("jobs")]
    public class BatchJobController : ControllerBase
    {
        private readonly IBatchService batchService;
        private readonly ILogger<BatchJobController> logger;

        public BatchJobController(
            IBatchService batchService,
            ILogger<BatchJobController> logger)
        {
            this.batchService = batchService;
            this.logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CreateJobAsync([FromBody] CreateBatchJobRequest request)
        {
            try
            {
                await this.batchService.CreateJobAsync(request.JobId, request.PoolId);
                return this.Accepted();
            }
            catch (BatchServiceException batchServiceException)
            {
                this.logger.LogError(
                    batchServiceException,
                    batchServiceException.Message);

                return this.BadRequest(batchServiceException.Message);
            }
        }

        [HttpDelete]
        [Route("{jobId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteJobAsync(string jobId)
        {
            try
            {
                await this.batchService.DeleteJobAsync(jobId);
                return this.NoContent();
            }
            catch (BatchServiceException batchServiceException)
            {
                this.logger.LogError(
                    batchServiceException,
                    batchServiceException.Message);

                return this.BadRequest(batchServiceException.Message);
            }
        }

        [HttpPost]
        [Route("{jobId}/tasks")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CreateTasksAsync(
            string jobId,
            [FromBody] CreateBatchTasksRequest request)
        {
            try
            {
                await this.batchService.CreateJobTasksAsync(
                    jobId,
                    request.InputContainerName,
                    request.EventHubConnectionString,
                    request.EventHubName,
                    request.ApplicationPackage);

                return this.Accepted();
            }
            catch (BatchServiceException batchServiceException)
            {
                this.logger.LogError(
                    batchServiceException,
                    batchServiceException.Message);

                return this.BadRequest(batchServiceException.Message);
            }
        }
    }
}
