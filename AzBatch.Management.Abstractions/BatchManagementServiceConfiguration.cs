// <copyright file="BatchManagementServiceConfiguration.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Abstractions
{
    public class BatchManagementServiceConfiguration
    {
        public string BatchAccountName { get; set; }

        public string ResourceGroupName { get; set; }
    }
}
