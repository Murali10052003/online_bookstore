using onlineBookStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IGenreDAO
    {
        List<Genre> GetAllGenres();
        Genre GetGenreById(int genreId);
        bool AddGenre(string genreName); // Optional (Admin)
    }

}
