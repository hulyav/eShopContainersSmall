using System;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Buying.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
