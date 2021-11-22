namespace AzBatch.Abstractions
{
    public class BatchServiceConnectionConfiguration
    {
        public string EndpointUri { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }
}