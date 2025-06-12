using Microsoft.AspNetCore.Components;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using YumBlazor.Models;
using YumBlazor.Repository;
using YumBlazor.Utility;

namespace YumBlazor.Services
{
    public class PaymentService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IRepository<OrderHeader> _orderRepository;
 

        public PaymentService(
            NavigationManager navigationManager,
            IRepository<OrderHeader> orderRepository
              )
        {
            _navigationManager = navigationManager;
            _orderRepository = orderRepository;
        }

        public Session CreateStripeCheckoutSession(OrderHeader orderHeader)
        {
           // var stripeApiKey = _configuration["StripeApiKey"];

            var lineItems = orderHeader.OrderDetails
                   .Select(od => new SessionLineItemOptions
                   {
                       PriceData = new SessionLineItemPriceDataOptions
                       {
                           Currency = "usd",
                           ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                           {
                               Name = od.ProductName
                           },
                           UnitAmountDecimal = (decimal?)(od.Price * 100), // Convert to cents
                       },
                       Quantity = od.Count,
                   }).ToList();

            var options = new Stripe.Checkout.SessionCreateOptions
                    {
                        SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                        CancelUrl = $"{_navigationManager.BaseUri}cart",
                        LineItems = lineItems,
                        Mode = "payment",
                    };
                   
            var service = new SessionService();
            Session session = service.Create(options);
            return session; 
        }

        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            var orderHeader = (await _orderRepository.GetAllAsync()).FirstOrDefault(o => o.SessionId == sessionId); 
            if (orderHeader != null)
            {
                var service = new SessionService(); 
                var session = await service.GetAsync(sessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    orderHeader.Status = SD.StatusApproved;  
                    orderHeader.PaymentIntentId = session.PaymentIntentId;
                    await _orderRepository.UpdateAsync(orderHeader);
                }
               
            }
            return orderHeader??new OrderHeader();
        }
    }
}
