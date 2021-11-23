// <copyright file="CreateApplicationPackageResponse.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    using System;

    public class CreateApplicationPackageResponse
    {
        public string StorageUrl { get; set; }
        public DateTime? StorageUrlExpiresOn { get; set; }
        public string ApplicationPackageState { get; set; }
    }
}