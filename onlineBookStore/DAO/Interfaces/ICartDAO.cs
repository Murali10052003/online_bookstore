using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    using onlineBookStore.Entity;
    

    public interface ICartDAO
    {
        bool AddOrUpdateCartItem(int userId, int bookId, int quantity);
        bool RemoveFromCart(int userId, int bookId);
        List<Cart> GetCartItemsByUser(int userId);
        bool ClearCart(int userId);
    }

}
