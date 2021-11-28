// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.WordCounter
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
            Console.WriteLine($"Reading file '{options.InputFile}'...");

            var inputText = await File.ReadAllTextAsync(options.InputFile);

            var wordCount = inputText
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Length;

            Console.WriteLine($"Count is '{wordCount}'");

            var ehClient = new EventHubProducerClient(options.EventHubConnectionString, options.EventHubName);
            var batch = await ehClient.CreateBatchAsync();
            var message = new EventData(Encoding.UTF8.GetBytes($"{options.InputFile}\n{wordCount.ToString()}"));

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