using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using onlineBookStore.DAO.Interfaces;
using onlineBookStore.Entity;
using onlineBookStore.Util;
namespace onlineBookStore.DAO.Implementations
{
    public class UserImpl: IUserDAO

    {
        private readonly DbConnectionUtil _dbUtil;
        public UserImpl (DbConnectionUtil dbUtil)
        {  _dbUtil = dbUtil; }
        public bool Register (User user)
        { using (SqlConnection conn= _dbUtil.GetOpenConnection()) {
                string query = "INSERT INTO USERS (Username,Email,PasswordHash) VALUES (@Username,@Email,@PasswordHash)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public User Login(string email, string password)
        {
            using (SqlConnection conn = _dbUtil.GetOpenConnection())
            {
                string query = "SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        UserID = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        
                    };
                }
                return null;
            }
        }
    }



}
            




   
