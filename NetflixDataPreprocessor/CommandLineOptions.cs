// <copyright file="CommandLineOptions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.NetflixDataPreprocessor
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option(longName: "input-file", shortName: 'f', Required = true, HelpText = "The input CSV file.")]
        public string InputFile { get; set; }

        [Option(longName: "output", shortName: 'o', Required = true, HelpText = "The output directory.")]
        public string OutputDirectory { get; set; }
    }
}