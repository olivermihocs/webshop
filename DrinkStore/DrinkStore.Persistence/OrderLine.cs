using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class OrderLine
    {
        //A rendelések egy sora, termék/rendelés kulcspárral azonosítjuk

        [Key, ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Packaging Packaging { get; set; }
    }
}
