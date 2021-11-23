// <copyright file="ApplicationPackagesNotEmptyException.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Infrastructure
{
    using System;

    public class ApplicationPackagesNotEmptyException : Exception
    {
        public ApplicationPackagesNotEmptyException()
            : base("Application cannot be deleted because it contains packages. Delete all packages prior to deleting the application.")
        {
        }
    }
}
