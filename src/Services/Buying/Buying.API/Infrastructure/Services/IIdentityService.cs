using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Buying.API.Infrastructure.Services
{
    public interface IIdentityService
    {
        string GetUserIdentity();
    }
}
