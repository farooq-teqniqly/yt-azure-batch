// <copyright file="BatchPoolConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Abstractions
{
    public class BatchPoolConfiguration
    {
        public string Id { get; set; }

        public int TargetDedicatedComputeNodes { get; set; }
    }
}