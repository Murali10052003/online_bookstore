﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    using onlineBookStore.Entity;
    

    public interface ICartDAO
    {
        bool AddToCart(CartItem item);
        bool UpdateCartQuantity(int userId, int bookId, int quantity);
        bool RemoveFromCart(int userId, int bookId);
        List<CartItem> GetCartItemsByUser(int userId);
        bool ClearCart(int userId);
    }

}
