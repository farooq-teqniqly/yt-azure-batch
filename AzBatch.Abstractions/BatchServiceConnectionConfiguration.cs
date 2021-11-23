// <copyright file="BatchServiceConnectionConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Abstractions
{
    public class BatchServiceConnectionConfiguration
    {
        public string EndpointUri { get; set; }

        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}