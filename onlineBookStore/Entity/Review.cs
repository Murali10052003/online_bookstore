using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Entity
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public int Rating { get; set; } // 1 to 5
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Username { get; set; }
        public string Comment { get;  set; }
        public DateTime ReviewDate { get;  set; }
    }

}
