using YumBlazor.Models;

namespace YumBlazor.Utility
{
    public static class SD
    {
        public readonly static string Role_Admin = "Admin";
        public readonly static string Role_Customer = "Customer";

        public readonly static string StatusPending = "Pending";
        public readonly static string StatusApproved = "Approved";
        public readonly static string StatusReadyForPickup = "ReadyForPickup";
        public readonly static string StatusCompleted = "Completed";
        public readonly static string StatusCancelled = "Cancelled";

        public static List<OrderDetail> ConvertShoppingCartListToOrderDetail(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    Price = (double)item.Product.Price,
                    ProductName = item.Product.Name,    
                };
                orderDetails.Add(orderDetail);
            }
            return orderDetails;
        }   
    }
}
