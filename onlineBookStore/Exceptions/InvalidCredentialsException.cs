using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Exceptions
{
    public class InvalidCredentialsException:Exception
    {
        //public InvalidCredentialsException(string message): base(message) { }
       
            public InvalidCredentialsException() : base("Invalid email or password.") { }

            public InvalidCredentialsException(string message) : base(message) { }

            public InvalidCredentialsException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }


