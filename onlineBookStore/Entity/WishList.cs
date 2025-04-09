using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.Entity
{
    public class WishlistItem
    {
        public int WishlistID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
