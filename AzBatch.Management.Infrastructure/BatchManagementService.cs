// <copyright file="BatchManagementService.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Management.Batch;
    using Microsoft.Rest.Azure;
    using Teqniqly.AzBatch.Management.Abstractions;

    public sealed class BatchManagementService : IBatchManagementService, IDisposable
    {
        private readonly BatchManagementServiceConfiguration configuration;
        private readonly IBatchManagementClient batchManagementClient;

        public BatchManagementService(
            BatchManagementServiceConfiguration configuration,
            IBatchManagementClient batchManagementClient)
        {
            this.configuration = configuration;
            this.batchManagementClient = batchManagementClient;
        }

        public async Task<ApplicationPackageStorage> CreateApplicationPackageAsync(
            string applicationPackageName,
            string applicationPackageVersion)
        {
            var result = await this
                .batchManagementClient
                .ApplicationPackage
                .CreateAsync(
                    this.configuration.ResourceGroupName,
                    this.configuration.BatchAccountName,
                    applicationPackageName,
                    applicationPackageVersion);

            return new ApplicationPackageStorage
            {
                Url = result.StorageUrl,
                State = result.State.ToString(),
                ExpiresOn = result.StorageUrlExpiry,
            };
        }

        public async Task DeleteApplicationPackageAsync(
            string applicationPackageName,
            string applicationPackageVersion)
        {
            await this
                .batchManagementClient
                .ApplicationPackage
                .DeleteAsync(
                    this.configuration.ResourceGroupName,
                    this.configuration.BatchAccountName,
                    applicationPackageName,
                    applicationPackageVersion);
        }

        public async Task DeleteApplicationAsync(string applicationName)
        {
            try
            {
                await this
                        .batchManagementClient
                        .Application
                        .DeleteAsync(
                            this.configuration.ResourceGroupName,
                            this.configuration.BatchAccountName,
                            applicationName);
            }
            catch (CloudException cloudException)
            {
                throw new BatchManagementServiceException(cloudException);
            }
        }

        public void Dispose()
        {
            this.batchManagementClient?.Dispose();
        }
    }
}
