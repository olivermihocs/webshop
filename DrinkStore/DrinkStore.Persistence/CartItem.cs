using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;

namespace DrinkStore.Persistence
{
    public class CartItem
    {

        //Ez az osztály tárolja a munkamenethez tartozó kosárban lévő termékhez tartozó fontos információkat
        public CartItem(int productId, int quantity, Packaging packaging)
        {
            ProductId = productId;
            Quantity = quantity;
            Packaging = packaging;
        }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Packaging Packaging { get; set; }
    }
}
