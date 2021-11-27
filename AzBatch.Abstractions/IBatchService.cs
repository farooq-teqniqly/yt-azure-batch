// <copyright file="IBatchService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBatchService
    {
        Task CreatePoolAsync(
            BatchPoolConfiguration batchPoolConfiguration,
            ImageReferenceConfiguration imageReferenceConfiguration,
            VmConfiguration vmConfiguration,
            IList<ApplicationPackage> applicationPackages);

        Task<string> GetPoolAllocationStateAsync(string poolId);

        Task DeletePoolAsync(string poolId);

        Task CreateJobAsync(string jobId, string poolId);

        Task DeleteJobAsync(string jobId);

        Task CreateJobTasksAsync(
            string jobId,
            string inputContainerName,
            string eventHubConnectionString,
            string eventHubName,
            ApplicationPackage applicationPackage);
    }
}
