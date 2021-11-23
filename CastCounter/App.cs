// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.CastCounter
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class App
    {
        public async Task RunAsync(CommandLineOptions options)
        {
            Console.WriteLine($"Reading file '{options.InputFile}'...");

            var inputText = await File.ReadAllTextAsync(options.InputFile);

            var castCount = inputText
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Length;

            await File.WriteAllTextAsync(
                options.OutputFile,
                castCount.ToString());

            Console.WriteLine($"Output written to '{options.OutputFile}'.");
        }
    }
}