// <copyright file="BatchServiceConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Abstractions
{
    public class BatchServiceConfiguration
    {
        public BatchServiceConnectionConfiguration ConnectionConfiguration { get; set; }
        public BatchServiceStorageConfiguration StorageConfiguration { get; set; }
    }

    public class BatchServiceStorageConfiguration
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }
}