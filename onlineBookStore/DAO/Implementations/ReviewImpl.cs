using Microsoft.Data.SqlClient;
using onlineBookStore.DAO.Interfaces;
using onlineBookStore.Entity;
using onlineBookStore.Util;
using onlineBookStore.Util;
using System.Collections.Generic;

public class ReviewImpl : IReviewDAO
{
    private readonly DbConnectionUtil _dbUtil;

    public ReviewImpl(DbConnectionUtil dbUtil)
    {
        _dbUtil = dbUtil;
    }

    public bool AddReview(Review review)
    {
        using (var conn = _dbUtil.GetOpenConnection())
        {
            string query = "INSERT INTO Reviews (UserID, BookID, Rating, ReviewText) VALUES (@UserID, @BookID, @Rating, @ReviewText)";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", review.UserID);
            cmd.Parameters.AddWithValue("@BookID", review.BookID);
            cmd.Parameters.AddWithValue("@Rating", review.Rating);
            cmd.Parameters.AddWithValue("@ReviewText", review.ReviewText ?? "");

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public List<Review> GetReviewsByBookId(int bookId)
    {
        List<Review> reviews = new List<Review>();

        using (var conn = _dbUtil.GetOpenConnection())
        {
            string query = @"SELECT r.*, u.Username 
                             FROM Reviews r 
                             INNER JOIN Users u ON r.UserID = u.UserID 
                             WHERE r.BookID = @BookID 
                             ORDER BY r.CreatedAt DESC";

            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookID", bookId);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reviews.Add(new Review
                {
                    ReviewID = (int)reader["ReviewID"],
                    UserID = (int)reader["UserID"],
                    BookID = (int)reader["BookID"],
                    Rating = (int)reader["Rating"],
                    ReviewText = reader["ReviewText"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    Username = reader["Username"].ToString()
                });
            }
        }

        return reviews;
    }
}