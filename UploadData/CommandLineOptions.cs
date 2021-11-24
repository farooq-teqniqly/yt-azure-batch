// <copyright file="CommandLineOptions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.UploadData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;

    public class CommandLineOptions
    {
        [Option(longName: "storage-account-name", Required = true, HelpText = "The Azure blob storage account name.")]
        public string StorageAccountName { get; set; }

        [Option(longName: "storage-account-key", Required = true, HelpText = "The Azure blob storage account key.")]
        public string StorageAccountKey { get; set; }

        [Option(longName: "container", shortName: 'c', Required = true, HelpText = "The target container name. It must already exist.")]
        public string StorageContainerName { get; set; }

        [Option(longName: "local-directory", shortName: 'd', Required = true, HelpText = "The local directory containing the files to upload.")]
        public string LocaDirectory { get; set; }

        [Option(longName: "filter", shortName: 'f', Required = true, HelpText = "The file filter pattern.")]
        public string FileFilter { get; set; }
    }
}
