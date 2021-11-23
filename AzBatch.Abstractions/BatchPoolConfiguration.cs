namespace Teqniqly.AzBatch.Abstractions
{
    public class BatchPoolConfiguration
    {
        public string Id { get; set; }
        public int TargetDedicatedComputeNodes { get; set; }
    }
}