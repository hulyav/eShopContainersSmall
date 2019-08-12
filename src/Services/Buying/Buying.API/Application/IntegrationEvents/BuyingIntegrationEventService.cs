using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Microsoft.eShopOnContainers.Services.Buying.Infrastructure;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Buying.API.Application.IntegrationEvents
{
    public class BuyingIntegrationEventService : IBuyingIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly BuyingContext _buyingContext;
        private readonly IIntegrationEventLogService _eventLogService;

        public BuyingIntegrationEventService(IEventBus eventBus, BuyingContext buyingContext,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _buyingContext = buyingContext ?? throw new ArgumentNullException(nameof(buyingContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_buyingContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            await SaveEventAndBuyingContextChangesAsync(evt);
            _eventBus.Publish(evt);
            await _eventLogService.MarkEventAsPublishedAsync(evt);
        }

        private async Task SaveEventAndBuyingContextChangesAsync(IntegrationEvent evt)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_buyingContext)
                .ExecuteAsync(async () => {
                    // Achieving atomicity between original buying database operation and the IntegrationEventLog thanks to a local transaction
                    await _buyingContext.SaveChangesAsync();
                    await _eventLogService.SaveEventAsync(evt, _buyingContext.Database.CurrentTransaction.GetDbTransaction());
                });
        }
    }
}
