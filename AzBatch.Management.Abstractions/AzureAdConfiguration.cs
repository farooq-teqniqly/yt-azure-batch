// <copyright file="AzureAdConfiguration.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Abstractions
{
    public class AzureAdConfiguration
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/";

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SubscriptionId { get; set; }
    }
}
