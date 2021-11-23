﻿namespace Teqniqly.AzBatch.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Batch;
    using Microsoft.Azure.Batch.Auth;
    using Microsoft.Azure.Batch.Common;
    using Teqniqly.AzBatch.Abstractions;

    public sealed class BatchService : IBatchService, IDisposable
    {
        private readonly BatchServiceConfiguration configuration;
        private BatchClient client;

        public BatchService(BatchServiceConfiguration configuration)
        {
            this.configuration = configuration;
            this.Connect();
        }

        private void Connect()
        {
            BatchSharedKeyCredentials credentials = new BatchSharedKeyCredentials(
                this.configuration.ConnectionConfiguration.EndpointUri,
                this.configuration.ConnectionConfiguration.AccountName,
                this.configuration.ConnectionConfiguration.AccountKey);

            this.client = BatchClient.Open(credentials);
        }

        public async Task CreatePoolAsync(
            BatchPoolConfiguration batchPoolConfiguration,
            ImageReferenceConfiguration imageReferenceConfiguration,
            VmConfiguration vmConfiguration,
            IList<ApplicationPackage> applicationPackages)
        {
            var azBatchVmConfig = new VirtualMachineConfiguration(
                new ImageReference(
                    imageReferenceConfiguration.Offer,
                    imageReferenceConfiguration.Publisher,
                    imageReferenceConfiguration.Sku,
                    imageReferenceConfiguration.Version),
                vmConfiguration.NodeAgentSkuId);

            var pool = this.client.PoolOperations.CreatePool(
                batchPoolConfiguration.Id,
                vmConfiguration.Size,
                azBatchVmConfig);

            pool.ApplicationPackageReferences = applicationPackages?.Select(
                p => new ApplicationPackageReference
            {
                ApplicationId = p.Id,
                Version = p.Version,
            }).ToList();

            try
            {
                await pool.CommitAsync();
            }
            catch (BatchException batchException)
            {
                if (batchException.RequestInformation?.BatchError?.Code != BatchErrorCodeStrings.PoolExists)
                {
                    throw;
                }
            }
        }

        public async Task<string> GetPoolAllocationStateAsync(string poolId)
        {
            var allocationState = (await this
                    .client
                    .PoolOperations
                    .GetPoolAsync(poolId))
                .AllocationState;

            return allocationState.ToString();
        }

        public async Task DeletePoolAsync(string poolId)
        {
            try
            {
                await this.client.PoolOperations.DeletePoolAsync(poolId);
            }
            catch (BatchException batchException)
            {
                if (batchException.RequestInformation?.BatchError?.Code != BatchErrorCodeStrings.PoolNotFound)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            this.client?.Dispose();
        }
    }
}
