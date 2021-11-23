// <copyright file="Program.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.CastCounter
{
    using System;
    using System.Threading.Tasks;
    using CommandLine;

    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var app = new App();

            return await Parser
                .Default
                .ParseArguments<CommandLineOptions>(args)
                .MapResult(
                    async options =>
                    {
                        try
                        {
                            await app.RunAsync(options);
                            return 0;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            return -2;
                        }
                    },
                    errs => Task.FromResult(-1));
        }
    }
}
