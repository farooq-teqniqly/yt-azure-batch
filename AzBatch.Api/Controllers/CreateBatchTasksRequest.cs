using Teqniqly.AzBatch.Abstractions;

namespace Teqniqly.AzBatch.Api.Controllers
{
    public class CreateBatchTasksRequest
    {
        public string InputContainerName { get; set; }
        public string OutputContainerName { get; set; }
        public ApplicationPackage ApplicationPackage { get; set; }
    }
}