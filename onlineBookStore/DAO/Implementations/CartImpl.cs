using System.Data.SqlClient;
using OnlineBookstore.Entity;
using OnlineBookstore.Util;
using OnlineBookstore.DAO.Interfaces;

public class CartDAOImpl : ICartDAO
{
    public bool AddOrUpdateCartItem(int userId, int bookId, int quantity)
    {
        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            // Check if item already exists in cart
            string checkQuery = "SELECT Quantity FROM Cart WHERE UserID = @UserID AND BookID = @BookID";
            SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@UserID", userId);
            checkCmd.Parameters.AddWithValue("@BookID", bookId);
            object result = checkCmd.ExecuteScalar();

            if (result != null)
            {
                // Update existing quantity
                int newQuantity = (int)result + quantity;
                string updateQuery = "UPDATE Cart SET Quantity = @Quantity WHERE UserID = @UserID AND BookID = @BookID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Quantity", newQuantity);
                updateCmd.Parameters.AddWithValue("@UserID", userId);
                updateCmd.Parameters.AddWithValue("@BookID", bookId);
                return updateCmd.ExecuteNonQuery() > 0;
            }
            else
            {
                // Insert new item
                string insertQuery = "INSERT INTO Cart (UserID, BookID, Quantity) VALUES (@UserID, @BookID, @Quantity)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@UserID", userId);
                insertCmd.Parameters.AddWithValue("@BookID", bookId);
                insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                return insertCmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool RemoveFromCart(int userId, int bookId)
    {
        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            string query = "DELETE FROM Cart WHERE UserID = @UserID AND BookID = @BookID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public List<CartItem> GetCartItemsByUser(int userId)
    {
        List<CartItem> cartItems = new List<CartItem>();
        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            string query = @"SELECT c.CartID, c.BookID, c.Quantity, c.CreatedAt, b.Title, b.Price
                             FROM Cart c
                             INNER JOIN Books b ON c.BookID = b.BookID
                             WHERE c.UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            SqlDataReader reader = cmd.ExecuteReader();

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
        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            string query = "DELETE FROM Cart WHERE UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
