using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Util
{
    public class DbPropertyUtil
    {
        public static string GetConnectionString()
        {
            return "Server=10.4.132.9;Database=online_bookstore_7337;TrustServerCertificate=True;Trusted_Connection=True";
        }
    }
}
