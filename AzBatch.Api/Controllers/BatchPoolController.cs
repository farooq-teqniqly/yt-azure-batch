// <copyright file="BatchPoolController.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teqniqly.AzBatch.Abstractions;
    using Teqniqly.AzBatch.Infrastructure;

    [ApiController]
    [Route("pools")]
    public class BatchPoolController : ControllerBase
    {
        private readonly IBatchService batchService;
        private readonly ILogger<BatchPoolController> logger;

        public BatchPoolController(
            IBatchService batchService,
            ILogger<BatchPoolController> logger)
        {
            this.batchService = batchService;
            this.logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBatchPoolAsync([FromBody] CreateBatchPoolRequest request)
        {
            try
            {
                await this.batchService.CreatePoolAsync(
                    request.BatchPoolConfiguration,
                    request.ImageReferenceConfiguration,
                    request.VmConfiguration,
                    request.ApplicationPackages?.ToList());

                return this.Accepted();
            }
            catch (BatchServiceException batchApiException)
            {
                if (!batchApiException.PoolExists())
                {
                    this.logger.LogError(
                        batchApiException,
                        batchApiException.Message);

                    return this.BadRequest(batchApiException.Message);
                }

                return this.Accepted();
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    exception.Message);

                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{poolId}/state")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBatchPoolStateAsync(string poolId)
        {
            try
            {
                var state = await this.batchService.GetPoolAllocationStateAsync(poolId);
                return this.Ok(state);
            }
            catch (BatchServiceException batchApiException)
            {
                if (batchApiException.PoolNotFound())
                {
                    return this.NotFound();
                }

                this.logger.LogError(
                    batchApiException,
                    batchApiException.Message);

                return this.BadRequest(batchApiException.Message);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    exception.Message);

                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{poolId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBatchPoolAsync(string poolId)
        {
            try
            {
                await this.batchService.DeletePoolAsync(poolId);
                return this.NoContent();
            }
            catch (BatchServiceException batchApiException)
            {
                if (batchApiException.PoolNotFound())
                {
                    return this.NoContent();
                }

                this.logger.LogError(
                    batchApiException,
                    batchApiException.Message);

                return this.BadRequest(batchApiException.Message);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    exception.Message);

                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
