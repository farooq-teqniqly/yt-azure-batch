// <copyright file="DeleteApplicationPackageRequest.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api.Controllers
{
    public class DeleteApplicationPackageRequest
    {
        public string ApplicationPackageName { get; set; }
        public string ApplicationPackageVersion { get; set; }
    }
}