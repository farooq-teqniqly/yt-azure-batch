// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.CastCounter
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;

    public class App
    {
        public async Task RunAsync(CommandLineOptions options)
        {
            Console.WriteLine($"Reading file '{options.InputFile.Trim()}'...");

            var inputText = await File.ReadAllTextAsync(options.InputFile);

            var castCount = inputText
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Length;

            Console.WriteLine($"Count is '{castCount}'");

            var ehClient = new EventHubProducerClient(options.EventHubConnectionString, options.EventHubName);
            var batch = await ehClient.CreateBatchAsync();
            var message = new EventData(Encoding.UTF8.GetBytes(castCount.ToString()));

            try
            {
                if (!batch.TryAdd(message))
                {
                    throw new Exception("Event is too large for the batch and cannot be sent.");
                }

                await ehClient.SendAsync(batch);

                Console.WriteLine("Message sent.");
            }
            finally
            {
                await ehClient.DisposeAsync();
            }
        }
    }
}