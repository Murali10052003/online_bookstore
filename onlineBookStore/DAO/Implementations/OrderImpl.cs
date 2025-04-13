using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using onlineBookStore.DAO.Interfaces;
using onlineBookStore.Entity;
using onlineBookStore.Exceptions;
using onlineBookStore.Util;
using onlineBookStore.Exceptions;

namespace onlineBookStore.DAO.Implementations
{
    public class OrderImpl : IOrderDAO
    {
        private readonly DbConnectionUtil _dbUtil;

        public OrderImpl(DbConnectionUtil dbUtil)
        {
            _dbUtil = dbUtil;
        }

        public int PlaceOrder(Order order, List<OrderItem> orderItems)
        {
            int orderId = -1;
            using (SqlConnection conn = _dbUtil.GetOpenConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Step 1: Insert into Orders table
                    string orderQuery = "INSERT INTO Orders (UserID, OrderDate, Status) OUTPUT INSERTED.OrderID VALUES (@UserID, @OrderDate, @Status)";
                    SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction);
                    orderCmd.Parameters.AddWithValue("@UserID", order.UserID);
                    orderCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    orderCmd.Parameters.AddWithValue("@Status", order.Status);

                    orderId = (int)orderCmd.ExecuteScalar();

                    // Step 2: Insert each item into OrderItems
                    foreach (var item in orderItems)
                    {
                        string stockQuery = "SELECT Stock FROM Books WHERE BookID = @BookID";
                        SqlCommand stockCmd = new SqlCommand(stockQuery, conn, transaction);
                        stockCmd.Parameters.AddWithValue("@BookID", item.BookID);
                        int currentStock = (int)stockCmd.ExecuteScalar();

                        if (item.Quantity > currentStock)
                        {
                            throw new InsufficientInventoryException($"❌ Book ID {item.BookID} has only {currentStock} in stock.");
                        }

                        string itemQuery = "INSERT INTO OrderItems (OrderID, BookID, Quantity, Price) VALUES (@OrderID, @BookID, @Quantity, @Price)";
                        SqlCommand itemCmd = new SqlCommand(itemQuery, conn, transaction);
                        itemCmd.Parameters.AddWithValue("@OrderID", orderId);
                        itemCmd.Parameters.AddWithValue("@BookID", item.BookID);
                        itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@Price", item.Price);
                        itemCmd.ExecuteNonQuery();

                        // Step 3: Update book stock
                        string updateStock = "UPDATE Books SET Stock = Stock - @Quantity WHERE BookID = @BookID";
                        SqlCommand stockCmd1 = new SqlCommand(updateStock, conn, transaction);
                        stockCmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                        stockCmd1.Parameters.AddWithValue("@BookID", item.BookID);
                        stockCmd1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Order Failed: " + ex.Message);
                    transaction.Rollback();
                    return -1;
                }
            }
            return orderId;
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = _dbUtil.GetOpenConnection())
            {
                string query = "SELECT * FROM Orders WHERE UserID = @UserID ORDER BY OrderDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        OrderDate = (DateTime)reader["OrderDate"],
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return orders;
        }

        public List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();
            using (SqlConnection conn = _dbUtil.GetOpenConnection())
            {
                string query = @"SELECT oi.*, b.Title FROM OrderItems oi
                             INNER JOIN Books b ON oi.BookID = b.BookID
                             WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new OrderItem
                    {
                        OrderItemID = (int)reader["OrderItemID"],
                        OrderID = (int)reader["OrderID"],
                        BookID = (int)reader["BookID"],
                        Quantity = (int)reader["Quantity"],
                        Price = (decimal)reader["Price"],
                        //BookTitle = reader["Title"].ToString()
                    });
                }
            }
            return items;

        }
    }
}