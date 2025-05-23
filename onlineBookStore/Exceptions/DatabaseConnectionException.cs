﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Exceptions
{
       
        public class DatabaseConnectionException : Exception
        {
            public DatabaseConnectionException() : base("Error connecting to the database.") { }

            public DatabaseConnectionException(string message) : base(message) { }

            public DatabaseConnectionException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }



