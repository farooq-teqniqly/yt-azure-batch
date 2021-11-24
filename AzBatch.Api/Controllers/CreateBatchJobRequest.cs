// <copyright file="CreateBatchJobRequest.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api.Controllers
{
    public class CreateBatchJobRequest
    {
        public string JobId { get; set; }

        public string PoolId { get; set; }
    }
}