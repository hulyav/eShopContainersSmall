
namespace Ordering.API.Application.IntegrationEvents.EventHandling
{
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Services;
    using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
    using Microsoft.Extensions.Logging;
    using Ordering.API.Application.IntegrationEvents.Events;
    using System;
    using System.Threading.Tasks;

    public class VerifiedOrAddedPaymentMethodIntegrationEventHandler : IIntegrationEventHandler<VerifiedOrAddedPaymentMethodIntegrationEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;


        public VerifiedOrAddedPaymentMethodIntegrationEventHandler(ILoggerFactory logger, IOrderRepository orderRepository, IIdentityService identityService)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Domain Logic comment:
        // When the Buyer and Buyer's payment method have been created or verified that they existed, 
        // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
        public async Task Handle(VerifiedOrAddedPaymentMethodIntegrationEvent @event)
        {
            var orderToUpdate = await _orderRepository.GetAsync(@event.OrderId);
            orderToUpdate.SetBuyerId(@event.BuyerId);
            orderToUpdate.SetPaymentId(@event.PaymentId);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            _logger.CreateLogger(nameof(VerifiedOrAddedPaymentMethodIntegrationEventHandler))
                .LogTrace($"Order with Id: {@event.OrderId} has been successfully updated with a payment method id: { @event.PaymentId }");
        }
    }
}
