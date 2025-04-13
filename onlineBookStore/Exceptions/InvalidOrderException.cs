using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Exceptions
{
    public class InvalidOrderException
    {
        public InvalidOrderException(string message) : base(message) { }
    }
}
