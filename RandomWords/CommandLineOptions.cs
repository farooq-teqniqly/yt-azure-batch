// <copyright file="CommandLineOptions.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.RandomWords
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;

    public class CommandLineOptions
    {
        [Option(longName: "file-count", Required = false, HelpText = "The number of files to create.")]
        public short FileCount { get; set; } = 1;

        [Option(longName: "min-words", Required = false, HelpText = "The minimum number of words per file.")]
        public byte MinWordsPerFile { get; set; } = 1;

        [Option(longName: "max-words", Required = false, HelpText = "The maximum number of words per file.")]
        public byte MaxWordsPerFile { get; set; } = 20;

        [Option(longName: "output-folder", Required = true, HelpText = "The output folder.")]
        public string OutputFolder { get; set; }
    }
}
