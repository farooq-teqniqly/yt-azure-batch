// <copyright file="CreateApplicationPackageRequest.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    public class CreateApplicationPackageRequest
    {
        public string ApplicationPackageName { get; set; }

        public string ApplicationPackageVersion { get; set; }
    }
}