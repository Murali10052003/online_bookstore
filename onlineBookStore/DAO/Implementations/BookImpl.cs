using Microsoft.Data.SqlClient;
using onlineBookStore.DAO.Interfaces;
using onlineBookStore.Entity;
using onlineBookStore.Util;
using System.Collections.Generic;

public class BookDAOImpl : IBookDAO
{
    private readonly DbConnectionUtil _dbUtil;

    public BookDAOImpl(DbConnectionUtil dbUtil)
    {
        _dbUtil = dbUtil;
    }

    public List<Book> GetBooksByGenre(int genreId)
    {
        List<Book> books = new List<Book>();
        using (SqlConnection conn = _dbUtil.GetOpenConnection())
        {
            string query = "SELECT * FROM Books WHERE GenreID = @GenreID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GenreID", genreId);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(MapToBook(reader));
            }
        }
        return books;
    }



    public Book GetBookById(int bookId)
    {
        using (SqlConnection conn = _dbUtil.GetOpenConnection())
        {
            string query = "SELECT * FROM Books WHERE BookID = @BookID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookID", bookId);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapToBook(reader);
            }
        }
        return null;
    }

    public List<Book> GetAllBooks()
    {
        List<Book> books = new List<Book>();
        using (SqlConnection conn = _dbUtil.GetOpenConnection())
        {
            string query = "SELECT * FROM Books";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(MapToBook(reader));
            }
        }
        return books;
    }

    private Book MapToBook(SqlDataReader reader)
    {
        return new Book
        {
            BookID = (int)reader["BookID"],
            Title = reader["Title"].ToString(),
            Author = reader["Author"].ToString(),
            Price = (decimal)reader["Price"],
            Stock = (int)reader["Stock"],
            GenreID = (int)reader["GenreID"],
           // CreatedAt = (DateTime)reader["CreatedAt"]
        };
    }
}