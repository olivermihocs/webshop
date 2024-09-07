using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;

namespace DrinkStore.Web.Models
{
    public class CategoryViewModel
    {
        //Nézetmodell egy alkategória megjelenítéséhez

        //Termékek listája
        public IEnumerable<Product> Products { get; set; }

        //Kategória neve
        public String CategoryName { get; set; }

        //Jelenlegi oldal
        public Int32 CurrentPage { get; set; }

        //Van-e előző oldal
        public bool PrevPageAvailable { get; set; }

        //Van-e következő oldal?
        public bool NextPageAvailable { get; set; }

        //Rendezés
        public bool OrderByManufacturer { get; set; }
        public bool OrderByPrice { get; set; }
        public bool Reverse { get; set; }

        //ÁFA
        public Int32 VAT { get; set; }


    }
}
