// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.UploadData
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Azure.Storage;
    using Azure.Storage.Blobs;

    public class App
    {
        public async Task RunAsync(CommandLineOptions options)
        {
            var filePaths = Directory.GetFiles(options.LocaDirectory, options.FileFilter);

            if (filePaths.Length == 0)
            {
                Console.WriteLine("No files found matching the specified pattern.");
                return;
            }

            var blobServiceClient = new BlobServiceClient(
                new Uri($"https://{options.StorageAccountName}.blob.core.windows.net"),
                new StorageSharedKeyCredential(
                    options.StorageAccountName,
                    options.StorageAccountKey));

            var containerClient = blobServiceClient.GetBlobContainerClient(options.StorageContainerName);

            foreach (var filePath in filePaths)
            {
                var blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

                Console.WriteLine($"Uploading '{filePath}' to '{blobClient.Uri}'...");

                await blobClient.UploadAsync(filePath, true);

                Console.WriteLine($"Uploaded '{filePath}' to '{blobClient.Uri}'.");
            }
        }
    }
}