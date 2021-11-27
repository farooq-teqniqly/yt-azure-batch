// <copyright file="Extensions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Infrastructure
{
    using System;

    public static class Extensions
    {
        public static bool ApplicationPackagesExist(this BatchManagementServiceException batchManagementServiceException)
        {
            return string.Compare(
                batchManagementServiceException
                    .NativeCloudException
                    .Body
                    .Code,
                "ApplicationPackagesNotEmpty",
                StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
