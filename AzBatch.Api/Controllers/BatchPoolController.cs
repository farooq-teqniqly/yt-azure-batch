namespace Teqniqly.AzBatch.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Batch;
    using Microsoft.Azure.Batch.Common;
    using Microsoft.Extensions.Logging;
    using Teqniqly.AzBatch.Abstractions;

    [ApiController]
    [Route("pool")]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(BatchError))]
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
            catch (BatchException batchException)
            {
                var batchError = batchException.RequestInformation?.BatchError;
                this.logger.LogError(batchError?.ToString());
                return this.BadRequest(batchException.RequestInformation?.BatchError);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception.ToString());
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{poolId}/state")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BatchError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBatchPoolStateAsync(string poolId)
        {
            try
            {
                var state = await this.batchService.GetPoolAllocationStateAsync(poolId);
                return this.Ok(state);
            }
            catch (BatchException batchException)
            {
                var batchError = batchException.RequestInformation?.BatchError;
                this.logger.LogError(batchError?.ToString());
                return this.BadRequest(batchException.RequestInformation?.BatchError);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception.ToString());
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{poolId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BatchError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBatchPoolAsync(string poolId)
        {
            try
            {
                await this.batchService.DeletePoolAsync(poolId);
                return this.NoContent();
            }
            catch (BatchException batchException)
            {
                var batchError = batchException.RequestInformation?.BatchError;
                this.logger.LogError(batchError?.ToString());
                return this.BadRequest(batchException.RequestInformation?.BatchError);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception.ToString());
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
