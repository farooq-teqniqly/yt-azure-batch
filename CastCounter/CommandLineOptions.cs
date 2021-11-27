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

        [Option(longName: "event-hub-connection-string", Required = true, HelpText = "The target event hub namespace connection string.")]
        public string EventHubConnectionString { get; set; }

        [Option(longName: "event-hub-name", Required = true, HelpText = "The target event hub name.")]
        public string EventHubName { get; set; }
    }
}
