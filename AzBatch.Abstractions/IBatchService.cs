namespace Teqniqly.AzBatch.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBatchService
    {
        Task CreatePoolAsync(
            BatchPoolConfiguration batchPoolConfiguration,
            ImageReferenceConfiguration imageReferenceConfiguration,
            VmConfiguration vmConfiguration,
            IList<ApplicationPackage> applicationPackages);

        Task<string> GetPoolAllocationStateAsync(string poolId);

        Task DeletePoolAsync(string poolId);
    }
}
