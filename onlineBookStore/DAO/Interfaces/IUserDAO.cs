using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using onlineBookStore.Entity;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IUserDAO
    {
        bool Register(User user);
        User Login(string email, string password);
        //User GetUserById(int userId);
        //bool IsEmailExists(string email);
    }
}
