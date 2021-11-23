// <copyright file="CommandLineOptions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.CastCounter
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option(longName: "input-file", shortName: 'f', Required = true, HelpText = "The input file.")]
        public string InputFile { get; set; }

        [Option(longName: "output-file", shortName: 'o', Required = true, HelpText = "The output file.")]
        public string OutputFile { get; set; }
    }
}
