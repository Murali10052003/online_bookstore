using onlineBookStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IWishlistDAO
    {
        bool AddToWishlist(int userId, int bookId);
        bool RemoveFromWishlist(int userId, int bookId);
        List<WishlistItem> GetWishlistItemsByUser(int userId);
    }
}
