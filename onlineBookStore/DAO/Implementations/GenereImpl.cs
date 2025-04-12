public class GenreDAOImpl : IGenreDAO
{
    private readonly DbConnectionUtil _dbUtil;

    public GenreDAOImpl(DbConnectionUtil dbUtil)
    {
        _dbUtil = dbUtil;
    }

    public List<Genre> GetAllGenres()
    {
        List<Genre> genres = new List<Genre>();
        using (SqlConnection conn = _dbUtil.GetOpenConnection())
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
        using (SqlConnection conn = _dbUtil.GetOpenConnection())
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
