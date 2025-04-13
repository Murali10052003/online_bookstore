using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Exceptions
{
    public class InsufficientInventoryException : Exception
    {
        public InsufficientInventoryException() { }

        public InsufficientInventoryException(string message) : base(message) { }

    }
}