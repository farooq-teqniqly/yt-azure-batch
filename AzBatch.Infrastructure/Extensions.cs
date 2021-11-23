// <copyright file="Extensions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Infrastructure
{
    using Microsoft.Azure.Batch.Common;

    public static class Extensions
    {
        public static bool PoolNotFound(this BatchApiException batchApiException)
        {
            return batchApiException
                .NativeException
                .RequestInformation?
                .BatchError?
                .Code == BatchErrorCodeStrings.PoolNotFound;
        }

        public static bool PoolExists(this BatchApiException batchApiException)
        {
            return batchApiException
                .NativeException
                .RequestInformation?
                .BatchError?
                .Code == BatchErrorCodeStrings.PoolExists;
        }
    }
}
