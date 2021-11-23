// <copyright file="BatchManagementServiceException.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Infrastructure
{
    using System;
    using Microsoft.Rest.Azure;

    public class BatchManagementServiceException : Exception
    {
        public BatchManagementServiceException(CloudException cloudException)
        :base(cloudException.Body.Message)
        {
            this.NativeCloudException = cloudException;
        }

        public CloudException NativeCloudException { get; set; }
    }
}
