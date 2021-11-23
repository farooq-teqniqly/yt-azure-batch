// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.NetflixDataPreprocessor
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Sylvan.Data;
    using Sylvan.Data.Csv;

    public class App
    {
        public async Task RunAsync(CommandLineOptions options)
        {
            var csv = await CsvDataReader.CreateAsync(options.InputFile);

            foreach (var record in csv.GetRecords<CsvRecord>())
            {
                if (string.IsNullOrWhiteSpace(record.Cast))
                {
                    continue;
                }

                var titleKebabCased = string.Join(
                        "-",
                        record.Title.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                    .ToLowerInvariant()
                    .Replace(":", string.Empty);

                try
                {
                    await File.WriteAllTextAsync(
                                Path.Join(
                                    options.OutputDirectory,
                                    $"{titleKebabCased}.txt"),
                                record.Cast);
                }
                catch (IOException)
                {
                }
            }
        }
    }
}