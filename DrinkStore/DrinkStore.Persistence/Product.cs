using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    //Termék
    public class Product
    {
        //Modellszám
        [Key]
        public Int32 Id { get; set; }

        //Gyártó
        public String Manufacturer { get; set; }

        public String TypeNo { get; set; }

        //Leírás
        public String Description { get; set; }

        //Kategória
        [Required]
        public Int32 CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //Nettó ár
        public Int32 Price { get; set; }

        //Készlet
        public Int32 Stock { get; set; }

        //Csomagolás
        public Packaging Packaging { get; set; }
    }
}
