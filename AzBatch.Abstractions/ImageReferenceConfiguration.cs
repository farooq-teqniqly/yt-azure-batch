namespace AzBatch.Abstractions
{
    public class ImageReferenceConfiguration
    {
        public string Offer { get; set; }
        public string Publisher { get; set; }
        public string Sku { get; set; }
        public string Version { get; set; } = "latest";
    }
}