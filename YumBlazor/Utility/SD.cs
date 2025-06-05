using YumBlazor.Models;

namespace YumBlazor.Utility
{
    public static class SD
    {
        public static string Role_Admin = "Admin";
        public static string Role_Customer = "Customer";

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickup = "ReadyForPickup";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";

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
