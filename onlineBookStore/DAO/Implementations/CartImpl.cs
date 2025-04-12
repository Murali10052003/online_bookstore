using Microsoft.Data.SqlClient;
using OnlineBookstore.Entity;
using OnlineBookstore.Util;
using System.Collections.Generic;

public class CartDAOImpl : ICartDAO
{
    private readonly DbConnectionUtil _dbUtil;

    public CartDAOImpl(DbConnectionUtil dbUtil)
    {
        _dbUtil = dbUtil;
    }

    public bool AddToCart(CartItem item)
    {
        using (var conn = _dbUtil.GetOpenConnection())
        {
            // Check if item exists
            string checkQuery = "SELECT COUNT(*) FROM Cart WHERE UserID = @UserID AND BookID = @BookID";
            var checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@UserID", item.UserID);
            checkCmd.Parameters.AddWithValue("@BookID", item.BookID);

            int exists = (int)checkCmd.ExecuteScalar();
            if (exists > 0)
            {
                // Already in cart, update quantity
                return UpdateCartQuantity(item.UserID, item.BookID, item.Quantity);
            }
            else
            {
                // Add new item
                string insertQuery = "INSERT INTO Cart (UserID, BookID, Quantity) VALUES (@UserID, @BookID, @Quantity)";
                var insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@UserID", item.UserID);
                insertCmd.Parameters.AddWithValue("@BookID", item.BookID);
                insertCmd.Parameters.AddWithValue("@Quantity", item.Quantity);

                return insertCmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool UpdateCartQuantity(int userId, int bookId, int quantity)
    {
        using (var conn = _dbUtil.GetOpenConnection())
        {
            string updateQuery = "UPDATE Cart SET Quantity = @Quantity WHERE UserID = @UserID AND BookID = @BookID";
            var cmd = new SqlCommand(updateQuery, conn);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool RemoveFromCart(int userId, int bookId)
    {
        using (var conn = _dbUtil.GetOpenConnection())
        {
            string deleteQuery = "DELETE FROM Cart WHERE UserID = @UserID AND BookID = @BookID";
            var cmd = new SqlCommand(deleteQuery, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public List<CartItem> GetCartItemsByUser(int userId)
    {
        List<CartItem> cartItems = new List<CartItem>();

        using (var conn = _dbUtil.GetOpenConnection())
        {
            string query = @"SELECT c.CartID, c.BookID, c.Quantity, c.CreatedAt, b.Title, b.Price
                             FROM Cart c
                             INNER JOIN Books b ON c.BookID = b.BookID
                             WHERE c.UserID = @UserID";

            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cartItems.Add(new CartItem
                {
                    CartID = (int)reader["CartID"],
                    BookID = (int)reader["BookID"],
                    Quantity = (int)reader["Quantity"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    BookTitle = reader["Title"].ToString(),
                    BookPrice = (decimal)reader["Price"]
                });
            }
        }

        return cartItems;
    }

    public bool ClearCart(int userId)
    {
        using (var conn = _dbUtil.GetOpenConnection())
        {
            string query = "DELETE FROM Cart WHERE UserID = @UserID";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
