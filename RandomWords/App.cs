// <copyright file="App.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.RandomWords
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class App
    {
        private string[] dictionary;

        public async Task RunAsync(CommandLineOptions options)
        {
            await this.LoadDictionary();

            var numFiles = options.FileCount;

            for (var i = 0; i < numFiles; i++)
            {
                var fileName = Path.Join(
                    options.OutputFolder,
                    $"{Guid.NewGuid():N}.txt");

                var words = this.GetRandomWords(
                    options.MinWordsPerFile,
                    options.MaxWordsPerFile);

                await File.WriteAllTextAsync(
                    fileName,
                    string.Join(",", words));

                Console.WriteLine($@"Wrote {words.Length} words to '{fileName}'. {i + 1} of {numFiles}");
            }
        }

        private async Task LoadDictionary()
        {
            this.dictionary = await File.ReadAllLinesAsync("dictionary.txt");
        }

        private string[] GetRandomWords(int minWords, int maxWords)
        {
            var count = new Random().Next(minWords, maxWords);
            var words = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var randomIndex = new Random().Next(0, this.dictionary.Length);
                words.Add(this.dictionary[randomIndex]);
            }

            return words.ToArray();
        }
    }
}