namespace Microsoft.eShopOnContainers.Services.Buying.API.Application.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBuyerQueries
    {
        //Task<Order> GetOrderAsync(int id);

        //Task<IEnumerable<OrderSummary>> GetOrdersAsync();

        Task<IEnumerable<CardType>> GetCardTypesAsync();
    }
}
