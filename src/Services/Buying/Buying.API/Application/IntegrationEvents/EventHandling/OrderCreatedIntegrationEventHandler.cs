

namespace Buying.API.Application.IntegrationEvents.EventHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
    //    using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.OrderAggregate;
    using Buying.API.Application.IntegrationEvents.Events;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate;
    using Microsoft.eShopOnContainers.Services.Buying.API.Infrastructure.Services;

    public class OrderCreatedIntegrationEventHandler
         : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IIdentityService _identityService;
        private readonly IBuyingIntegrationEventService _buyingIntegrationEventService;

        public OrderCreatedIntegrationEventHandler(ILoggerFactory logger, IBuyerRepository buyerRepository, 
            IIdentityService identityService, IBuyingIntegrationEventService buyingIntegrationEventService)
        {
            _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _buyingIntegrationEventService = buyingIntegrationEventService ?? throw new ArgumentNullException(nameof(buyingIntegrationEventService));
        }

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            var cardTypeId = (@event.CardTypeId != 0) ? @event.CardTypeId : 1;
            var buyer = await _buyerRepository.FindAsync(@event.UserId);
            bool buyerOriginallyExisted = (buyer == null) ? false : true;

            if (!buyerOriginallyExisted)
            {
                buyer = new Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer(@event.UserId);
            }

            var payment = buyer.VerifyOrAddPaymentMethod(cardTypeId,
                                           $"Payment Method on {DateTime.UtcNow}",
                                           @event.CardNumber,
                                           @event.CardSecurityNumber,
                                           @event.CardHolderName,
                                           @event.CardExpiration,
                                           @event.OrderId);

            var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.Add(buyer);

            await _buyerRepository.UnitOfWork
                .SaveEntitiesAsync();

            //int paymentId = 1;//buyer.PaymentMethods.FirstOrDefault().Id;

            var verifiedOrAddedPaymentMethodIntegrationEvent = new VerifiedOrAddedPaymentMethodIntegrationEvent(buyer.Id, payment.Id, @event.OrderId);
            await _buyingIntegrationEventService.PublishThroughEventBusAsync(verifiedOrAddedPaymentMethodIntegrationEvent);

            _logger.CreateLogger(nameof(OrderCreatedIntegrationEventHandler)).LogTrace($"Buyer {buyerUpdated.Id} and related payment method were validated or updated for orderId: {@event.OrderId}.");
        }


        //public OrderPaymentFailedIntegrationEventHandler(IOrderRepository orderRepository)
        //{
        //    _orderRepository = orderRepository;
        //}

        //public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
        //{
        //    var orderToUpdate = await _orderRepository.GetAsync(@event.OrderId);

        //    orderToUpdate.SetCancelledStatus();

        //    await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        //}
    }
}


//namespace Buying.API.Application.IntegrationEvents.EventHandling
//{
//    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
//    using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.OrderAggregate;
//    using Buying.API.Application.IntegrationEvents.Events;
//    using System.Threading.Tasks;

//    public class OrderPaymentFailedIntegrationEventHandler : 
//        IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
//    {
//        private readonly IOrderRepository _orderRepository;

//        public OrderPaymentFailedIntegrationEventHandler(IOrderRepository orderRepository)
//        {
//            _orderRepository = orderRepository;
//        }

//        public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
//        {
//            var orderToUpdate = await _orderRepository.GetAsync(@event.OrderId);

//            orderToUpdate.SetCancelledStatus();

//            await _orderRepository.UnitOfWork.SaveEntitiesAsync();
//        }
//    }
//}
