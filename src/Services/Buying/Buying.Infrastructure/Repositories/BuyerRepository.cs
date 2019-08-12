using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate;
using Microsoft.eShopOnContainers.Services.Buying.Domain.Seedwork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Buying.Infrastructure.Repositories
{
    public class BuyerRepository
        : Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.IBuyerRepository
    {
        private readonly BuyingContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public BuyerRepository(BuyingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer Add(
            Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                return _context.Buyers
                    .Add(buyer)
                    .Entity;
            }
            else
            {
                return buyer;
            }
        }

        public Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer Update(
            Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer buyer)
        {
            return _context.Buyers
                    .Update(buyer)
                    .Entity;
        }

        public async Task<Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer> FindAsync(string identity)
        {
            var buyer = await _context.Buyers
                .Include(b => b.PaymentMethods)
                .Where(b => b.IdentityGuid == identity)
                .SingleOrDefaultAsync();

            return buyer;
        }
    }
}
