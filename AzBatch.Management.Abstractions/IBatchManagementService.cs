// <copyright file="IBatchManagementService.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Abstractions
{
    using System.Threading.Tasks;

    public interface IBatchManagementService
    {
        Task<ApplicationPackageStorage> CreateApplicationPackageAsync(
            string applicationPackageName,
            string applicationPackageVersion);

        Task DeleteApplicationPackageAsync(
            string applicationPackageName,
            string applicationPackageVersion);

        Task DeleteApplicationAsync(string applicationName);
    }
}
