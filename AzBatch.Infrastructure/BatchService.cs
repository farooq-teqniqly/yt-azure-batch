// <copyright file="BatchService.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

using System.IO;
using Azure.Storage.Blobs;

namespace Teqniqly.AzBatch.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Batch;
    using Microsoft.Azure.Batch.Auth;
    using Microsoft.Azure.Batch.Common;
    using Teqniqly.AzBatch.Abstractions;

    public sealed class BatchService : IBatchService, IDisposable
    {
        private readonly BatchClient batchClient;
        private readonly BlobServiceClient blobServiceClient;

        public BatchService(
            BatchClient batchClient,
            BlobServiceClient blobServiceClient)
        {
            this.batchClient = batchClient;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task CreatePoolAsync(
            BatchPoolConfiguration batchPoolConfiguration,
            ImageReferenceConfiguration imageReferenceConfiguration,
            VmConfiguration vmConfiguration,
            IList<ApplicationPackage> applicationPackages)
        {
            var azBatchVmConfig = new VirtualMachineConfiguration(
                new ImageReference(
                    imageReferenceConfiguration.Offer,
                    imageReferenceConfiguration.Publisher,
                    imageReferenceConfiguration.Sku,
                    imageReferenceConfiguration.Version),
                vmConfiguration.NodeAgentSkuId);

            var pool = this.batchClient.PoolOperations.CreatePool(
                batchPoolConfiguration.Id,
                vmConfiguration.Size,
                azBatchVmConfig,
                batchPoolConfiguration.TargetDedicatedComputeNodes);

            pool.ApplicationPackageReferences = applicationPackages?.Select(
                p => new ApplicationPackageReference
            {
                ApplicationId = p.Id,
                Version = p.Version,
            }).ToList();

            try
            {
                await pool.CommitAsync();
            }
            catch (BatchException batchException)
            {
                throw new BatchServiceException(batchException);
            }
        }

        public async Task<string> GetPoolAllocationStateAsync(string poolId)
        {
            try
            {
                var allocationState = (await this
                            .batchClient
                            .PoolOperations
                            .GetPoolAsync(poolId))
                        .AllocationState;

                return allocationState.ToString();
            }
            catch (BatchException batchException)
            {
                throw new BatchServiceException(batchException);
            }
        }

        public async Task DeletePoolAsync(string poolId)
        {
            try
            {
                await this.batchClient.PoolOperations.DeletePoolAsync(poolId);
            }
            catch (BatchException batchException)
            {
                throw new BatchServiceException(batchException);
            }
        }

        public async Task CreateJobAsync(string jobId, string poolId)
        {
            try
            {
                var job = this.batchClient.JobOperations.CreateJob(
                    jobId,
                    new PoolInformation { PoolId = poolId });

                await job.CommitAsync();
            }
            catch (BatchException batchException)
            {
                throw new BatchServiceException(batchException);
            }
        }

        public async Task CreateJobTasksAsync(
            string jobId,
            string inputContainerName,
            string outputContainerName,
            ApplicationPackage applicationPackage)
        {
            var inputContainerClient = this.blobServiceClient.GetBlobContainerClient(inputContainerName);
            var blobItems = inputContainerClient.GetBlobsAsync();
            var inputResourceFiles = new List<ResourceFile>();
            var outputContainerClient = this.blobServiceClient.GetBlobContainerClient(outputContainerName);
            var outputContainer = new OutputFileBlobContainerDestination(outputContainerClient.Uri.ToString());

            await foreach (var blobItem in blobItems)
            {
                var blob = inputContainerClient.GetBlobClient(blobItem.Name);

                inputResourceFiles.Add(ResourceFile.FromUrl(
                    blob.Uri.ToString(),
                    blob.Name));
            }

            var tasks = new List<CloudTask>();

            for (var i = 0; i < inputResourceFiles.Count; i++)
            {
                var taskId = $"Task-{i}";
                var appPath = $"%AZ_BATCH_APP_PACKAGE_{applicationPackage.Id}#{applicationPackage.Version}%";
                var inputFile = inputResourceFiles[i].FilePath;
                var appOutputFile = $"{Path.GetFileNameWithoutExtension(inputResourceFiles[i].FilePath)}.out.txt";
                var taskCommandLine = $"cmd /c {appPath}\\cc.exe --input-file {inputFile} --output-file {appOutputFile}";

                var task = new CloudTask(taskId, taskCommandLine)
                {
                    ResourceFiles = new List<ResourceFile> {inputResourceFiles[i]},
                };

                var outputFiles = new List<OutputFile>();
                
                var outputFile = new OutputFile(
                    appOutputFile,
                    new OutputFileDestination(outputContainer),
                    new OutputFileUploadOptions(OutputFileUploadCondition.TaskSuccess));

                outputFiles.Add(outputFile);
                task.OutputFiles = outputFiles;
                tasks.Add(task);
            }

            try
            {
                await this.batchClient.JobOperations.AddTaskAsync(jobId, tasks);
            }
            catch (BatchException batchException)
            {
                throw new BatchServiceException(batchException);
            }
        }

        public void Dispose()
        {
            this.batchClient?.Dispose();
        }
    }
}
