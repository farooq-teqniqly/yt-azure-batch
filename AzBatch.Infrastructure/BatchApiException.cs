// <copyright file="BatchApiException.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Infrastructure
{
    using System;
    using Microsoft.Azure.Batch.Common;

    public class BatchApiException : Exception
    {
        public BatchApiException(BatchException batchException)
            : base(batchException.RequestInformation?.BatchError.Message.ToString())
        {
            this.NativeException = batchException;
        }

        public BatchException NativeException { get; }
    }
}
