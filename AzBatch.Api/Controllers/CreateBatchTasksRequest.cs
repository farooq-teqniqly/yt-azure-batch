// <copyright file="CreateBatchTasksRequest.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api.Controllers
{
    using Teqniqly.AzBatch.Abstractions;

    public class CreateBatchTasksRequest
    {
        public string EventHubConnectionString { get; set; }

        public string EventHubName { get; set; }

        public string InputContainerName { get; set; }

        public ApplicationPackage ApplicationPackage { get; set; }
    }
}