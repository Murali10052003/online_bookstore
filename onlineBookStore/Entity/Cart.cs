using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Entity
{
    public class Cart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BookTitle { get; set; }
        public decimal BookPrice { get; set; }
        
    }

}
