using Microsoft.Data.SqlClient;
using onlineBookStore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Util
{
    public class DbConnectionUtil
    {
        private readonly string _connectionString;
        public DbConnectionUtil(string connectionString)
        { _connectionString = connectionString; }
        public SqlConnection GetOpenConnection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                return conn;

            }
            catch(Exception ex) { 
                throw new DatabaseConnectionException("Unable to Connect to the database",ex);
            }
            
        }
    }
}
