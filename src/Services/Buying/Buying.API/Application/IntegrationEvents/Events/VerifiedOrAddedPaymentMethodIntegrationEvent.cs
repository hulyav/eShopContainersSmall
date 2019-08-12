

namespace Buying.API.Application.IntegrationEvents.Events
{
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

    public class VerifiedOrAddedPaymentMethodIntegrationEvent : IntegrationEvent
    {
        public int BuyerId { get; private set; }
        public int PaymentId { get; private set; }
        public int OrderId { get; private set; }

        public VerifiedOrAddedPaymentMethodIntegrationEvent(int buyerId, int paymentId, int orderId)
        {
            BuyerId = buyerId;
            PaymentId = paymentId;
            OrderId = orderId;
        }
    }
}
