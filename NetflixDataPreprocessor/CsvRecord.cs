// <copyright file="CsvRecord.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.NetflixDataPreprocessor
{
    public class CsvRecord
    {
        public string ShowId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public string Cast { get; set; }

        public string Country { get; set; }

        public string DateAdded { get; set; }

        public short ReleaseYear { get; set; }

        public string Rating { get; set; }

        public string Duration { get; set; }

        public string ListedIn { get; set; }

        public string Description { get; set; }
    }
}