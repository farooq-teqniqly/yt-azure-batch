// <copyright file="ApplicationPackageStorage.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Abstractions
{
    using System;

    public class ApplicationPackageStorage
    {
        public string Url { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public string State { get; set; }
    }
}
