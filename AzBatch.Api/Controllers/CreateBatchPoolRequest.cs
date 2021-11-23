namespace Teqniqly.AzBatch.Api.Controllers
{
    using Teqniqly.AzBatch.Abstractions;

    public class CreateBatchPoolRequest
    {
        public BatchPoolConfiguration BatchPoolConfiguration { get; set; }
        public ImageReferenceConfiguration ImageReferenceConfiguration { get; set; }
        public VmConfiguration VmConfiguration { get; set; }
        public ApplicationPackage[] ApplicationPackages { get; set; }
    }
}