using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OnlineBookstore.Entity;
using OnlineBookstore.Util;

public class GenreDAOImpl : IGenreDAO
{
    public List<Genre> GetAllGenres()
    {
        List<Genre> genres = new List<Genre>();

        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            string query = "SELECT * FROM Genres";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                genres.Add(new Genre
                {
                    GenreID = (int)reader["GenreID"],
                    GenreName = reader["GenreName"].ToString()
                });
            }
        }

        return genres;
    }

    public Genre GetGenreById(int genreId)
    {
        using (SqlConnection conn = DBConnectionUtil.GetConnection())
        {
            string query = "SELECT * FROM Genres WHERE GenreID = @GenreID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GenreID", genreId);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Genre
                {
                    GenreID = (int)reader["GenreID"],
                    GenreName = reader["GenreName"].ToString()
                };
            }
        }

        return null;
    }
}
