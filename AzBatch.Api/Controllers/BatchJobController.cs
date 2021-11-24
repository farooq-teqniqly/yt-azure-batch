// <copyright file="BatchJobController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api.Controllers
{
    using System.Threading.Tasks;
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
    }
}
