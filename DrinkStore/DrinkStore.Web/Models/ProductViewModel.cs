using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;

namespace DrinkStore.Web.Models
{
    public class ProductViewModel
    {

        //Termékhez tartozó nézetmodell

        public Product Product { get; set; }

        //Mennyiség
        public int Quantity { get; set; }

        //Választható csomagolások
        public List<SelectListItem> PackageList { get; set; }

        //Kiválasztott csomagolás
        public String SelectedPackaging { get; set; }

        //ÁFA
        public Int32 VAT { get; set; }
    }
}
