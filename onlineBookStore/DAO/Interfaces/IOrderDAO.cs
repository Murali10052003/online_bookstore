using onlineBookStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IOrderDAO
    {
        int PlaceOrder(Order order, List<OrderItem> orderItems);
        List<Order> GetOrdersByUser(int userId);
        List<OrderItem> GetOrderItems(int orderId);
        List<Order> GetAllOrders(); // Admin
        bool UpdateOrderStatus(int orderId, string status); // Admin
    }
}
