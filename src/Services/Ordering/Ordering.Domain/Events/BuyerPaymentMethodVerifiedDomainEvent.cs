using MediatR;
//using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Ordering.Domain.Events
{
    public class BuyerAndPaymentMethodVerifiedDomainEvent
        : INotification
    {
        public int BuyerId { get; private set; }
        public int PaymentId { get; private set; }
        public int OrderId { get; private set; }

        public BuyerAndPaymentMethodVerifiedDomainEvent(int buyerId, int paymentId, int orderId)
        {
            BuyerId = buyerId;
            PaymentId = paymentId;
            OrderId = orderId;
        }
    }
}
